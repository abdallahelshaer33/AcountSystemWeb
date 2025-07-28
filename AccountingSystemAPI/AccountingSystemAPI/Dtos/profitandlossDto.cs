namespace AccountingSystemAPI.Dtos
{
    public class profitandlossDto
    {
        public decimal TotalRevenue { get; set; }    // اجمالى الايرادات
        public decimal totalExpenses { get; set; }  // اجمالى المصروفات 
        public decimal Netprofit => TotalRevenue - totalExpenses;  // صافى  الربح 
    }
}