using System.Threading.Tasks;
using AccountingSystemAPI.Data;
using AccountingSystemAPI.Dtos;
using AccountingSystemAPI.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InvoiceController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _db.Invoices
            .Include(i => i.Customer)
            .Include(i => i.InvoiceItems)
            .Select(i => new
            {
                i.Id,
                i.CustomerId,
                i.InvoiceDate,
                i.TotalAmount,
                i.Notes,
                Items = i.InvoiceItems.Select(t =>new
                {
                    t.Id,
                    t.Description,
                    t.Quantity,
                    t.UnitPrice
                })
            })
            .ToListAsync();
            return Ok(invoices);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getById(int id)
        {
            var invoices = await _db.Invoices
            .Include(i => i.Customer)
            .Include(i => i.InvoiceItems)
            .Select(i => new
            {
                i.Id,
                i.CustomerId,
                i.InvoiceDate,
                i.TotalAmount,
                i.Notes,
                Items = i.InvoiceItems.Select(t =>new
                {
                    t.Id,
                    t.Description,
                    t.Quantity,
                    t.UnitPrice
                })
            })
            .FirstOrDefaultAsync(i => i.Id == id);
            if (invoices == null) return NotFound();
            return Ok(invoices);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceDto dto)
        {
            var invoice = new Invoice
            {
                CustomerId = dto.CustomerId,
                InvoiceDate = dto.InvoiceDate,
                TotalAmount = dto.TotalAmount,
                Notes = dto.Notes,
                InvoiceItems = dto.items.Select(i => new InvoiceItem
                {
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();
            return Ok(invoice);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceDto dto)
        {

            var existingInvoice = await _db.Invoices
         .Include(i => i.InvoiceItems)
         .FirstOrDefaultAsync(i => i.Id == id);
            if (existingInvoice == null)
                return NotFound();

            existingInvoice.CustomerId = dto.CustomerId;
            existingInvoice.InvoiceDate = dto.InvoiceDate;
            existingInvoice.TotalAmount = dto.TotalAmount;
            existingInvoice.Notes = dto.Notes;
            //حذف عناصر الفاتورة القديمة
            _db.InvoiceItems.RemoveRange(existingInvoice.InvoiceItems);
            //اضافة العناصر الجديده
            existingInvoice.InvoiceItems = dto.items.Select(it => new InvoiceItem
            {
                Description = it.Description,
                Quantity = it.Quantity,
                UnitPrice = it.UnitPrice
            }).ToList();
            await _db.SaveChangesAsync();
            return Ok(true);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoices = await _db.Invoices
            .Include(i =>i.InvoiceItems)
            .FirstOrDefaultAsync(i => i.Id == id);
            if (invoices == null) return NotFound();
            _db.InvoiceItems.RemoveRange(invoices.InvoiceItems);
            _db.Invoices.Remove(invoices);
            await _db.SaveChangesAsync();
            return NoContent();                                                         
        }

    }
}