namespace LuckySlots.App.Models
{
    using LuckySlots.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class DepositViewModel
    {
        [Required]
        public string CardId { get; set; }

        [Required]
        [Range(0,10000000, ErrorMessage ="Deposit amount cannot be negative.")]
        [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Amount field can contain only digits.")]
        public decimal Amount { get; set; }
        
        public ICollection<SelectListItem> CreditCards{ get; set; }
        
        public CreditCardViewModel CreditCardModel { get; set; }

    }
}
