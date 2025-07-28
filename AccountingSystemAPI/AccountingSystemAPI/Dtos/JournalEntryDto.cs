namespace AccountingSystemAPI.Dtos
{
    public class JournalEntryDto
    {
        public DateTime? EntryDate { get; set; }

        public string? Description { get; set; }
        public List<JournalDetailDto> Details { get; set; }
    }
    public class JournalDetailDto
    {
        public string AccountName { get; set; } = null!;

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

    }
}