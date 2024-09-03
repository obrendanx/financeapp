using System.ComponentModel.DataAnnotations;

namespace FinanceApp.ViewModels;

public class CalenderPayment
{
    public decimal? PaymentDecrease { get; set; }
    public decimal? PaymentIncrease { get; set; }
    public decimal? Balance { get; set; }
    [Key]
    public int? PaymentId { get; set; }
    public string? Email { get; set; }
    public string? PaymentName { get; set; }
    public decimal? PaymentTotal { get; set; }
    public string? PaymentDate { get; set; }
    public string? PaymentFreq { get; set; }
    public decimal? PaymentAmount { get; set; }
    public List<Payments>? Payments { get; set; }
}