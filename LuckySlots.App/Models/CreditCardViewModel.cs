﻿namespace LuckySlots.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class CreditCardViewModel
    {
        [Required]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Card number must have only digits.")]
        [MaxLength(19, ErrorMessage = "Card number must contain 16 digits.")]
        [MinLength(19, ErrorMessage = "Card number must contain 16 digits.")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "CVV must have only digits.")]
        [MaxLength(4, ErrorMessage = "CVV must have 3 or 4 digits.")]
        [MinLength(3, ErrorMessage = "CVV must have 3 or 4 digits.")]
        public string CVV { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yy}")]
        public DateTime Expiry { get; set; }

        //[Required]
        //[Range(1,31, ErrorMessage ="Invalid month.")]
        //public int Month { get; set; }

        //[Required]
        //[Range(2018,2100,ErrorMessage ="Invalid year.")]
        //public int Year { get; set; }
    }
}