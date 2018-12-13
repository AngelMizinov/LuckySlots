namespace LuckySlots.Services.Account
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Infrastructure.HttpClient;
    using LuckySlots.Infrastructure.Providers;
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
        private readonly IJsonParser jsonParser;

        public AccountService(
            LuckySlotsDbContext context,
            ITransactionServices transactionServices,
            ICreditCardService creditCardService,
            IJsonParser jsonParser)
                : base(context)
        {
            this.transactionServices = transactionServices;
            this.creditCardService = creditCardService;
            this.jsonParser = jsonParser;
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

            // TODO: Change all instances of DateTime.Now to DateTime.UtcNow
            var transaction = new Transaction
            {
                Date = DateTime.UtcNow,
                User = user,
                Type = type.ToString(),
                Amount = amount,
                Description = description
            };

            var createTransactionResult = await this.Context.Transactions.AddAsync(transaction);

            if (createTransactionResult == null)
            {
                // TODO: Create custom exception and Try/Catch it
                throw new Exception("Failed to add the new transaction to the DB.");
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

            if (user == null)
            {
                throw new UserDoesntExistsException("User does not exists!");
            }

            string description = "";
            double exchangeRate = 1;

            if (type == TransactionType.Deposit)
            {
                var cardNumber = await this.creditCardService.GetLastForDigitsOfCardNumberAsync(cardId);

                description = $"Deposit with card **** **** **** {cardNumber}";

                if (user.Currency != "USD")
                {
                    exchangeRate = await this.jsonParser.ExtractExchangeRate(user.Currency);
                }
            }
            else if (type == TransactionType.Win)
            {
                description = $"Win on game {gameName}";
            }

            var transaction = new Transaction
            {
                Date = DateTime.UtcNow,
                User = user,
                Type = type.ToString(),
                Amount = amount,
                BaseCurrency = "USD",
                BaseCurrencyAmount = amount / (decimal)exchangeRate,
                ExchangeRate = exchangeRate,
                QuotedCurrency = user.Currency,
                QuotedCurrencyAmount = amount,
                Description = description
            };

            var createTransactionResult = await this.Context.Transactions.AddAsync(transaction);

            if (createTransactionResult == null)
            {
                // TODO: Create custom exception and Try/Catch it
                throw new Exception("Failed to add the new transaction to the DB.");
            }

            user.AccountBalance += amount;
            await this.Context.SaveChangesAsync();

            return user.AccountBalance;
        }
    }
}
