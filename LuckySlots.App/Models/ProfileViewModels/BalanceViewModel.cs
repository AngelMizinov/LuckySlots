namespace LuckySlots.App.Models.ProfileViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class BalanceViewModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain digits.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain digits.")]
        public string LastName { get; set; }

        public decimal AccountBalance { get; set; }

        public DeleteCardViewModel DeleteCardModel { get; set; }

    }
}
