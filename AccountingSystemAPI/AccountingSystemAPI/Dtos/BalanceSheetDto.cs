namespace AccountingSystemAPI.Dtos
{
    public class BalanceSheetDto
    {
        public decimal TotalAssets { get; set; }     //الاصول
        public decimal TotalLiabilities { get; set; }    // الخصوم 
        public decimal TotalEquity { get; set; }  // عداله

        public List<AccountBalanceDto> Assets { get; set; } = new();
        public List<AccountBalanceDto> Liabilities { get; set; } = new();
        public List<AccountBalanceDto> Equity { get; set; } = new();
    }
}