using System;
using System.Collections.Generic;

namespace AccountingSystemAPI.Models;

public partial class JournalEntry
{
    public int Id { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<JournalDetail> JournalDetails { get; set; } = new List<JournalDetail>();
}
