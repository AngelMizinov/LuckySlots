namespace LuckySlots.Services.Account
{
    using LuckySlots.Data;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountService : BaseService, IAccountService
    {
        public AccountService(LuckySlotsDbContext context) : base(context)
        {
        }

        public async Task<decimal> ChargeAccountAsync(string userId, decimal amount)
        {
            var user = await this.Context.Users
                .FirstOrDefaultAsync(us => us.Id == userId);
            
            if(user.AccountBalance < amount)
            {
                throw new InsufficientFundsException("User's balance is not enough for charging.");
            }

            user.AccountBalance -= amount;
            
            await this.Context.SaveChangesAsync();

            return user.AccountBalance;
        }

        public async Task<decimal> CheckBalanceAsync(string userId)
        {
            return await this.Context.Users
                .Where(us => us.Id == userId)
                .Select(us => us.AccountBalance)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> DepositAsync(string userId, decimal amount)
        {
            var user = await this.Context.Users
                .FirstOrDefaultAsync(us => us.Id == userId);
            
            user.AccountBalance += amount;

            await this.Context.SaveChangesAsync();

            return user.AccountBalance;
        }
    }
}
