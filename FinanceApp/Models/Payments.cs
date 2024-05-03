using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class Payments
    {
        [Key]
        public int PaymentId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PaymentName { get; set; }
        [Required]
        public decimal PaymentTotal { get; set; }
        [Required]
        public string PaymentDate { get; set; }
        [Required]
        public string PaymentFreq { get; set; }
    }
}