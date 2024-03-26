using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class AccountSetup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public int AccountBalance { get; set; }
    }
}
