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
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "CVV must have only digits.")]
        public decimal Amount { get; set; }
        
        public ICollection<SelectListItem> CreditCards{ get; set; }
        
        public CreditCardViewModel CreditCardModel { get; set; }

    }
}
