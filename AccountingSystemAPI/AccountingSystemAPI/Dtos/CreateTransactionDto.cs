namespace AccountingSystemAPI.Dtos
{
    public class CreateTransactionDto
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public int DebitAccountId { get; set; }
        public int CreditAccountId { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
    }
}
