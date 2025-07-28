namespace AccountingSystemAPI.Dtos
{
    public class RegisterModel
    {
         public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? FullName { get; set; } 
    }
}