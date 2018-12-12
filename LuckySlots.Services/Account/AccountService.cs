namespace LuckySlots.Services.Account
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountService : BaseService, IAccountService
    {
        private readonly ITransactionServices transactionServices;
        private readonly ICreditCardService creditCardService;

        public AccountService(
            LuckySlotsDbContext context,
            ITransactionServices transactionServices,
            ICreditCardService creditCardService)
                : base(context)
        {
            this.transactionServices = transactionServices;
            this.creditCardService = creditCardService;
        }

        public async Task<decimal> ChargeAccountAsync(string userId, decimal amount, TransactionType type, string gameName = null)
        {
            var user = await this.Context.Users
                .FirstOrDefaultAsync(us => us.Id == userId);

            if (user == null)
            {
                throw new UserDoesntExistsException("User does not exists!");
            }

            if (amount > user.AccountBalance)
            {
                throw new InsufficientFundsException("User's balance is not enough for charging.");
            }


            string description = "";

            if (type == TransactionType.Stake)
            {
                description = $"Stake on game {gameName}";
            }
            else //check Withdrawal
            {

            }

            var transaction = new Transaction
            {
                Date = DateTime.UtcNow,
                User = user,
                Type = type.ToString(),
                Amount = amount,
                Description = description
            };

            user.AccountBalance -= amount;
            await this.Context.Transactions.AddAsync(transaction);
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

        public async Task<string> CheckCurrencyAsync(string userId)
        {
            return await this.Context.Users
                .Where(us => us.Id == userId)
                .Select(us => us.Currency)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> DepositAsync(
            string userId,
            decimal amount,
            TransactionType type,
            string cardId = null,
            string gameName = null)
        {
            var user = await this.Context.Users
                .FirstOrDefaultAsync(us => us.Id == userId);

            user.AccountBalance += amount;

            string description = "";

            if (type == TransactionType.Deposit)
            {
                var cardNumber = await this.creditCardService.GetLastForDigitsOfCardNumberAsync(cardId);

                description = $"Deposit with card **** **** **** {cardNumber}";
            }
            else if (type == TransactionType.Win)
            {
                description = $"Win on game {gameName}";
            }

            // TODO: call 2 times SaveChangesAsync() 
            await this.transactionServices.CreateAsync(user.Id, type, amount, description);
            await this.Context.SaveChangesAsync();

            return user.AccountBalance;
        }
    }
}
