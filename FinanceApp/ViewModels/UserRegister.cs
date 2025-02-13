using System.ComponentModel.DataAnnotations;

namespace FinanceApp.ViewModels;

public class UserRegister
{
    [Required]
    [StringLength(16, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 16 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
    public string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[A-Z])(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain at least one letter, one number, one uppercase letter, and one symbol.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
}