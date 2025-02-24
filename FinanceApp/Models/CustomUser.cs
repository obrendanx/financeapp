using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Models;

public class CustomUser : IdentityUser
{
    public int Id { get; set; }
    
    public string Email { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }

    public bool IsSetup { get; set; }
}