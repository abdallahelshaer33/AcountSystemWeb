namespace AccountingSystemAPI.Dtos
{
    public class InvoiceDto
    {
        public int CustomerId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }

        public List<InvoiceItemDto> items { get; set; } = new();

    }
    public class InvoiceItemDto
    {
        public string Description { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}