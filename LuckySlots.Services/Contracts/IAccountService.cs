namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Infrastructure.Enums;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAccountService
    {
        Task<decimal> DepositAsync(string userId, decimal amount, TransactionType type, string cardId = null);

        Task<decimal> CheckBalanceAsync(string userId);

        Task<decimal> ChargeAccountAsync(string userId, decimal amount, TransactionType type);
    }
}
