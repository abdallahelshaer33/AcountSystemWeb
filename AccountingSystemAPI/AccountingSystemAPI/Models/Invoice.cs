using System;
using System.Collections.Generic;

namespace AccountingSystemAPI.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
