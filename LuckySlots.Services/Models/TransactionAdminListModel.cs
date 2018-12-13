namespace LuckySlots.Services.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TransactionAdminListModel
    {
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        [RegularExpression("[a-zA-Z")]
        public string Type { get; set; }

        public string BaseCurrency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseCurrencyAmount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string UserName { get; set; }
    }
}
