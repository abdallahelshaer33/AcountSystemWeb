namespace AccountingSystemAPI.Dtos
{
    public class AccountBalanceDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public decimal OpeningBalance { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal CurrentBalance { get; set; }
       
               
    }
}