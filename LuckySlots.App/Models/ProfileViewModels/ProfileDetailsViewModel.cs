namespace LuckySlots.App.Models.ProfileViewModels
{
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileDetailsViewModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain digits.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain digits.")]
        public string LastName { get; set; }

        public DateTime DateBirth { get; set; }

        public string Currency { get; set; }

    }
}
