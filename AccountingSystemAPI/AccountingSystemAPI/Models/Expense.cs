using System;
using System.Collections.Generic;

namespace AccountingSystemAPI.Models;

public partial class Expense
{
    public int Id { get; set; }

    public DateTime? ExpenseDate { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }
}
