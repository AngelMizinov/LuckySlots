using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySlots.Services.Contracts
{
    public interface IAccountService
    {
        Task<decimal> DepositAsync(string userId, decimal amount);

        Task<decimal> CheckBalanceAsync(string userId);

        Task<decimal> ChargeAccountAsync(string userId, decimal amount);
    }
}
