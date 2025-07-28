
using System.Threading.Tasks;
using AccountingSystemAPI.Data;
using AccountingSystemAPI.Dtos;
using AccountingSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class JournalController : ControllerBase
    {
        private readonly AppDbContext _db;
        public JournalController(AppDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<IActionResult> Create(JournalEntryDto dto)
        {
            if (dto.Details.Sum(d => d.Debit) != dto.Details.Sum(d => d.Credit))
            {
                return BadRequest("القيد غير متوازن الدائن لا يساوي المدين");
            }
            var entry = new JournalEntry
            {
                EntryDate = dto.EntryDate,
                Description = dto.Description,
                JournalDetails = dto.Details.Select(d => new JournalDetail
                {
                    AccountName = d.AccountName,
                    Debit = d.Debit,
                    Credit = d.Credit

                }).ToList()
            };
            _db.JournalEntries.Add(entry);
            await _db.SaveChangesAsync();
            return Ok(new { message = "تم حفظ القيد بنجاح", entry.Id });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _db.JournalEntries
                .Include(j => j.JournalDetails)
                .Select(e =>new
                {
                    e.Id,
                    e.EntryDate,
                    e.Description,
                    details = e.JournalDetails.Select(d =>new
                    {
                        d.Id,
                        d.AccountName,
                        d.Debit,
                        d.Credit
                    }).ToList()

                })
                .OrderByDescending(e => e.EntryDate)
                .ToListAsync();

            return Ok(entries);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _db.JournalEntries
                .Include(j => j.JournalDetails)
                .Select(e =>new
                {
                    e.Id,
                    e.EntryDate,
                    e.Description,
                    details = e.JournalDetails.Select(d =>new
                    {
                        d.Id,
                        d.AccountName,
                        d.Debit,
                        d.Credit
                    }).ToList()

                })
                .FirstOrDefaultAsync(j => j.Id == id);

            if (entry == null) return NotFound();

            return Ok(entry);
        }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entry = await _db.JournalEntries
            .Include(j => j.JournalDetails)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (entry == null) return NotFound();

        _db.JournalDetails.RemoveRange(entry.JournalDetails);
        _db.JournalEntries.Remove(entry);
        await _db.SaveChangesAsync();

        return Ok(new { message = "تم حذف القيد" });
    }
       
    }
}