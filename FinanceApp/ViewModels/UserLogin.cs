using System.ComponentModel.DataAnnotations;

namespace FinanceApp.ViewModels;

public class UserLogin
{
    [Required]
    public string Username { get; set; }
    [Required]
    public decimal Password { get; set; }
}