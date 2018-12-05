namespace LuckySlots.Data.Models
{
    using LuckySlots.Data.Models.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class CreditCard : IAuditable, IDeletable
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [DataType(DataType.CreditCard)]
        public string Number { get; set; }

        public int CVV { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        public User User { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.Date)]
        public DateTime Expiry { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
