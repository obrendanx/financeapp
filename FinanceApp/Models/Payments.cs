﻿using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class Payments
    {
        [Key]
        public int? PaymentId { get; set; }
        public string? Email { get; set; }
        public string? PaymentName { get; set; }
        public decimal? PaymentTotal { get; set; }
        public string? PaymentDate { get; set; }
        public string? PaymentFreq { get; set; }
    }
}