using System;
using System.Collections.Generic;

namespace AccountingSystemAPI.Models;

public partial class InvoiceItem
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
