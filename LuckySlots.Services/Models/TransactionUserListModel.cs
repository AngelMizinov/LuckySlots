namespace LuckySlots.Services.Models
{
    using System;

    public class TransactionUserListModel
    {
        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string QuotedCurrency { get; set; }

        public decimal QuotedCurrencyAmount { get; set; }

        public string Description { get; set; }
    }
}
