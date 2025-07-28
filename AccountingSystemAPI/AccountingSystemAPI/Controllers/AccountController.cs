using System.Threading.Tasks;
using AccountingSystemAPI.Data;
using AccountingSystemAPI.Dtos;
using AccountingSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _db.accounts.ToListAsync();
            return Ok(accounts);
        }
        [HttpPost]
        public async Task<IActionResult> AddAccount(CreateAccountDto dto)
        {
            var acount = new Account
            {
                Name = dto.Name,
                Type = dto.Type,
                OpeningBalance = dto.OpeningBalance,
                CreateAt = DateTime.Now
            };
            _db.accounts.Add(acount);
            await _db.SaveChangesAsync();
            return Ok(acount);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Updateaccount(int id, UpdateAccountDto dto)
        {
            var acount = await _db.accounts.FindAsync(id);
            if (acount == null) return NotFound();
            acount.Name = dto.Name;
            acount.Type = dto.Type;
            acount.OpeningBalance = dto.OpeningBalance;
            await _db.SaveChangesAsync();
            return Ok(acount);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var acount = await _db.accounts.FindAsync(id);
            if (acount == null) return NotFound();
            _db.accounts.Remove(acount);
            await _db.SaveChangesAsync();
            return Ok(new
            {
                 Message = "Account Deleted .... "
            });
        }


    }
}