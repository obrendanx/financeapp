using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models;

public class Payment
{
    [Required]
    public int Amount { get; set; }
}