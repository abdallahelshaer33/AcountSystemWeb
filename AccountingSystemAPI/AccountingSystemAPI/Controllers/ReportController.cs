
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
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ReportController(AppDbContext db)
        {
            _db = db;
        }

        //تقرير حساب معين
        [HttpGet("Ledger/{accountId}")]
        public async Task<IActionResult> GetLedger(int accountId, DateTime? from = null, DateTime? To = null)
        {
            var trans = await _db.transactions
            .Where(t => (t.DebitAccountId == accountId || t.CreditAccountId == accountId) &&
            (from == null || t.Date >= from) && (To == null || t.Date <= To))
            .Include(t => t.DebitAccount)
            .Include(t => t.CreditAccount)
            .OrderBy(t => t.Date)
            .ToListAsync();

            var leadger = new List<LeadgerEntryDto>();
            decimal balance = 0;
            foreach (var tran in trans)
            {
                var entery = new LeadgerEntryDto
                {
                    Date = tran.Date,
                    Descreption = tran.Description,
                    debit = tran.DebitAccountId == accountId ? tran.Amount : 0,
                    credit = tran.CreditAccountId == accountId ? tran.Amount : 0

                };
                balance += entery.debit - entery.credit;
                entery.Balance = balance;
                leadger.Add(entery);
            }
            return Ok(leadger);
        }

        // تقرير إجمالى الارصده 
        [HttpGet("balances")]
        public async Task<IActionResult> GetAccountBalances()
        {
            var acount = await _db.accounts.ToListAsync();
            var trans = await _db.transactions.ToListAsync();
            var balances = acount.Select(account =>
            {
                var debit = trans
               .Where(t => t.DebitAccountId == account.Id)
               .Sum(t => t.Amount);

                var credit = trans
                .Where(t => t.CreditAccountId == account.Id)
                .Sum(t => t.Amount);
                var balance = account.OpeningBalance + (debit - credit);
                return new
                {
                    account.Id,
                    account.Name,
                    account.Type,
                    openingBalance = account.OpeningBalance,
                    debitTotal = debit,
                    CreditTotal = credit,
                    Balance = balance

                };
            });
            return Ok(balances);
        }

        [HttpGet("ledger")]
        public async Task<IActionResult> getLedger()
        {
            var ledger = await _db.transactions
            .Include(t => t.DebitAccount)
            .Include(t => t.CreditAccount)
            .OrderBy(t => t.Date)
            .Select(t => new
            {
                t.Id,
                t.Date,
                t.Description,
                DebitAcount = t.DebitAccount.Name,
                creditAccount = t.CreditAccount.Name,
                t.Amount

            }).ToListAsync();
            return Ok(ledger);
        }


        [HttpGet("profit-loss")]
        public async Task<IActionResult> GetProfitandLoss(DateTime? From = null, DateTime? To = null)
        {
            From ??= DateTime.MinValue;
            To ??= DateTime.MaxValue;

            //الايرادات خلال الفترة  
            var Totalrevenue = await _db.Invoices
            .Where(i => i.InvoiceDate >= From && i.InvoiceDate <= To)
            .SumAsync(e => (decimal?)e.TotalAmount) ?? 0;

            //المصروفات خلال الفترة  
            var totalexpenses = await _db.Expenses
            .Where(e => e.ExpenseDate >= From && e.ExpenseDate <= To)
            .SumAsync(e => (decimal?)e.Amount) ?? 0;

            var result = new profitandlossDto
            {
                TotalRevenue = Totalrevenue,
                totalExpenses = totalexpenses
            };
            return Ok(result);



        }


        [HttpGet("expense-by-category")]
        public async Task<IActionResult> Getexpensebycategory(DateTime? from = null, DateTime? to = null)
        {
            from ??= DateTime.MinValue;
            to ??= DateTime.MaxValue;
            var result = await _db.Expenses
            .Where(e => e.ExpenseDate >= from && e.ExpenseDate <= to)
            .GroupBy(e => e.Category)
            .Select(g => new ExpenseByCategoryDto
            {
                Category = g.Key,
                TotalAmount = g.Sum(e => e.Amount)
            }).ToListAsync();
            return Ok(result);
        }


        [HttpGet("account-balance")]
        public async Task<IActionResult> getAccountBalance()
        {
            var accounts = await _db.accounts
            .Select(a => new AccountBalanceDto
            {
                AccountId = a.Id,
                AccountName = a.Name,
                AccountType = a.Type,
                OpeningBalance = a.OpeningBalance,
                TotalDebit = _db.transactions
                .Where(t => t.DebitAccountId == a.Id)
                .Sum(t => (decimal?)t.Amount) ?? 0,
                TotalCredit = _db.transactions
                .Where(t => t.CreditAccountId == a.Id)
                .Sum(t => (decimal?)t.Amount) ?? 0
            }).ToListAsync();
            foreach (var acc in accounts)
            {
                acc.CurrentBalance = acc.OpeningBalance + acc.TotalDebit - acc.TotalCredit;
            }
            return Ok(accounts);
        }

        [HttpGet("Balance-sheet")]
        public async Task<IActionResult> getbalanceSheet()
        {
            var accounts = await _db.accounts
        .Select(a => new AccountBalanceDto
        {
            AccountId = a.Id,
            AccountType = a.Type,
            AccountName = a.Name,
            OpeningBalance = a.OpeningBalance,
            TotalDebit = _db.transactions
            .Where(t => t.DebitAccountId == a.Id)
            .Sum(t => (decimal?)t.Amount) ?? 0,

            TotalCredit = _db.transactions
            .Where(t => t.CreditAccountId == a.Id)
            .Sum(t => (decimal?)t.Amount) ?? 0
        }).ToListAsync();
            foreach (var acc in accounts)
            {
                acc.CurrentBalance = acc.OpeningBalance + acc.TotalDebit - acc.TotalCredit;
            }
            var asset = accounts.Where(a => a.AccountType.ToLower() == "asset").ToList();
            var liabilities = accounts.Where(a => a.AccountType.ToLower() == "Liabilities").ToList();
            var equity = accounts.Where(a => a.AccountType.ToLower() == "Equity").ToList();
            var report = new BalanceSheetDto
            {
                Assets = asset,
                Liabilities = liabilities,
                TotalAssets = asset.Sum(a => a.CurrentBalance),
                TotalLiabilities = liabilities.Sum(a => a.CurrentBalance),
                TotalEquity = equity.Sum(a => a.CurrentBalance)
            };
            return Ok(report);



        }

        [HttpPut("transaction/{id}")]
        public async Task<IActionResult> UpdateLeadger(int id, [FromBody] transactionDTO update)
        {
            var existing = await _db.transactions.FindAsync(id);
            if (existing == null)
            {
                return NotFound();

            }
            existing.Date = update.Date;
            existing.Description = update.Description;
            existing.Amount = update.Amount;
            existing.DebitAccountId = update.DebitAccountId;
            existing.CreditAccountId = update.CreditAccountId;
            existing.CreatedBy = update.CreatedBy;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }
        [HttpDelete("transaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var trans = await _db.transactions.FindAsync(id);
            if (trans == null)
            {
                return NotFound();

            }
            _db.transactions.Remove(trans);
            await _db.SaveChangesAsync();
            return Ok(new { Message = "Deleted Successfully .... " });
        }


      
    }
}