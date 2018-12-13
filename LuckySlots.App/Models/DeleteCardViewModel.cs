﻿namespace LuckySlots.App.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class DeleteCardViewModel
    {
        [Required]
        public string CardId { get; set; }

        public ICollection<SelectListItem> CreditCards { get; set; }

        public CreditCardViewModel CreditCardModel { get; set; }

    }
}
