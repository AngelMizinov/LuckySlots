using LuckySlots.App.Infrastructure.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySlots.App.Models
{
    public class ValidationPageViewModel
    {
        [Required]
        //[Display(Name = "Date of birth")]
        [MinimumAge(18, ErrorMessage = "You must be {0} years of age in order to continue.")]
        public DateTime DateOfBirth { get; set; }
        
    }
}
