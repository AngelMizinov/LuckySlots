using LuckySlots.App.Infrastructure.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySlots.App.Models
{
    public class ValidationModalViewModel
    {
        //[Required]
        ////[Display(Name = "Date of birth")]
        //[MinimumAge(18, ErrorMessage = "You must be {0} years of age in order to continue.")]
        //public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(1, 31, ErrorMessage = "Invalid day!Must be between 1 and 31.")]
        public int Day { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Invalid month!Must be between 1 and 12.")]
        public int Month { get; set; }

        [Required]
        [Range(1900, 2018, ErrorMessage = "Invalid year!Must be between 1900 and 2018.")]
        public int Year { get; set; }
    }
}
