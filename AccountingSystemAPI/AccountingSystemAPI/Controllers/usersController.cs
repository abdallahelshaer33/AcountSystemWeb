using System.Threading.Tasks;
using AccountingSystemAPI.Data;
using AccountingSystemAPI.Dtos;
using AccountingSystemAPI.Helper;
using AccountingSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using StoreManagementSystem.Dtos;

namespace AccountingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public usersController(AppDbContext db)
        {
            _db = db;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _db.Users.AnyAsync(u => u.Username == model.Username))
                return BadRequest("username is already Exists ");

            var PasswordHashing = PasswordHash.Hash(model.Password);
            var user = new User
            {
                Username = model.Username,
                PasswordHash = PasswordHashing,
                Role = model.Role,
                FullName = model.FullName,
                CreatedAt = DateTime.Now
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok("user register Successfully ");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == model.username);
            if (user == null || !PasswordHash.verify(model.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid Credintals ... ");
            }
            HttpContext.Session.SetInt32("userID", user.Id);
            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("role", user.Role);
            return Ok(new
            {
                message = "Login Success",
                role = user.Role,
                username = user.Username,
                userId = user.Id
            });

        }
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            var userID = HttpContext.Session.GetInt32("userID");
            var username = HttpContext.Session.GetString("username");
            var Role = HttpContext.Session.GetString("role");
            if (userID == null)
                return Unauthorized("No User is Logged In");

            return Ok(new { userID, username, Role });
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logout Successfully .. ");
        }
        [HttpGet("All")]
        [RoleAuthorized("Admin")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _db.Invoices.ToListAsync();
            return Ok(invoices);
        }
        [HttpGet("users")]
        public async Task<IActionResult> getusers()
        {
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }

    }
}