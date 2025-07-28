
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
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TransactionController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _db.transactions
            .Include(t => t.DebitAccount)
            .Include(t => t.CreditAccount)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
            return Ok(transactions);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionDto dto)
        {
            if (dto.DebitAccountId == dto.CreditAccountId)
            {
                return BadRequest("Debit and credit account cant be the same");
            }

            var trans = new Transaction
            {
                Date = dto.Date,
                Description = dto.Description,
                DebitAccountId = dto.DebitAccountId,
                CreditAccountId = dto.CreditAccountId,
                Amount = dto.Amount,
                CreatedBy = dto.CreatedBy

            };
            _db.transactions.Add(trans);
            await _db.SaveChangesAsync();
            return Ok(trans);
        }


    }
}