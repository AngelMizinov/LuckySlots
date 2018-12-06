namespace LuckySlots.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class Transaction
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        [RegularExpression("[a-zA-Z")]
        public string Type { get; set; }

        [Range(typeof(decimal), "0", "1000000")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
