namespace AccountingSystemAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Description { get; set; } = null!;

        public int DebitAccountId { get; set; }

        public int CreditAccountId { get; set; }

        public decimal Amount { get; set; }

        public int CreatedBy { get; set; }

        public virtual Account DebitAccount { get; set; } = null!;
        public virtual Account CreditAccount { get; set; } = null!;
    }
}
