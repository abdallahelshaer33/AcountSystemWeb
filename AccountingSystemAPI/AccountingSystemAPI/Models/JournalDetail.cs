using System;
using System.Collections.Generic;

namespace AccountingSystemAPI.Models;

public partial class JournalDetail
{
    public int Id { get; set; }

    public int JournalEntryId { get; set; }

    public string AccountName { get; set; } = null!;

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public virtual JournalEntry JournalEntry { get; set; } = null!;
}
