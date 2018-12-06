namespace LuckySlots.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        //[ScaffoldColumn(false)]
        //public override Guid Id { get => base.Id; set => base.Id = value; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Range(typeof(decimal),"0","1000000")]
        public decimal AccountBalance { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateBirth { get; set; }

        [Range(3, 3)]
        [RegularExpression("[A-Z]")]
        public string Currency { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
