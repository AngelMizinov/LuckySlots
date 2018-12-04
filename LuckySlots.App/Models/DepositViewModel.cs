namespace LuckySlots.App.Models
{
    using LuckySlots.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DepositViewModel
    {
        public string CardId { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }
    }
}
