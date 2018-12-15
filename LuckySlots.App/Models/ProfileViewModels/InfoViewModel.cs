namespace LuckySlots.App.Models.ProfileViewModels
{
    using LuckySlots.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InfoViewModel
    {
        public string UserId { get; set; }

        public BalanceViewModel BalanceModel { get; set; }

        public ProfileDetailsViewModel ProfileDetailsModel { get; set; }

        //public TransactionUserListModel TransactionsModel { get; set; }


    }
}
