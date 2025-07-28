namespace AccountingSystemAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string Type { get; set; } = null;
        public decimal OpeningBalance { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}