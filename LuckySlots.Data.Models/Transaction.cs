namespace LuckySlots.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        [RegularExpression("[a-zA-Z")]
        public string Type { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
