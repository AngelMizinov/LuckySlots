namespace LuckySlots.App.Models
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class CreditCardViewModel
    {
        [Required]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Card number must have only digits.")]
        [MaxLength(21, ErrorMessage = "Card number must contain 16 digits.")]
        [MinLength(17, ErrorMessage = "Card number must contain 16 digits.")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "CVV must have only digits.")]
        [MaxLength(4, ErrorMessage = "CVV must have 3 or 4 digits.")]
        [MinLength(3, ErrorMessage = "CVV must have 3 or 4 digits.")]
        public string CVV { get; set; }

        [Required]
        [Remote(action:"ValidateDateExpiry",controller:"Account")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}")]
        public DateTime Expiry { get; set; }
    }
}
