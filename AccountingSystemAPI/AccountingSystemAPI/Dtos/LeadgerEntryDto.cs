namespace AccountingSystemAPI.Dtos
{
    public class LeadgerEntryDto
    {
        public DateTime Date { get; set; }
        public string Descreption { get; set; } = null!;
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public decimal Balance { get; set; }
    }
}