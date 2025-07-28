namespace AccountingSystemAPI.Dtos
{
    public class CreateAccountDto
    {
        public string Name { get; set; } = null;
        public string Type { get; set; } = null;
        public decimal OpeningBalance { get; set; }
    }
    public class UpdateAccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string Type { get; set; } = null;
        public decimal OpeningBalance { get; set; }
    }
}