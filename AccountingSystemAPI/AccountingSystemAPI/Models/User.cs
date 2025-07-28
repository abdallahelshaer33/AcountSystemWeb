using System;
using System.Collections.Generic;

namespace AccountingSystemAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? FullName { get; set; }

    public DateTime? CreatedAt { get; set; }
}
