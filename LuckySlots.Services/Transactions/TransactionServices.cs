namespace LuckySlots.Services.Transactions
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using LuckySlots.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class TransactionServices : BaseService, ITransactionServices
    {
        private readonly UserManager<User> userManager;

        public TransactionServices(
            LuckySlotsDbContext context,
            UserManager<User> userManager)
            : base(context)
        {
            this.userManager = userManager ?? throw new ArgumentNullException("UserManager cannot be null. An instance of UserManager is required.");
        }

        // TODO: Remove CreateAsync and fix unit tests
        public async Task<Transaction> CreateAsync(string userId, TransactionType type, decimal amount, string description)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserDoesntExistsException("User does not exists!");
            }

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                User = user,
                Type = type.ToString(),
                Amount = amount,
                Description = description
            };

            await this.Context.Transactions.AddAsync(transaction);
            await this.Context.SaveChangesAsync();

            return transaction;
        }

        public Task<IQueryable<TransactionAdminListModel>> GetAllAsync()
            => Task.FromResult(this.Context
                .Transactions
                .Select(tr => new TransactionAdminListModel
                {
                    Date = tr.Date,
                    Type = tr.Type,
                    BaseCurrency = tr.BaseCurrency,
                    BaseCurrencyAmount = tr.BaseCurrencyAmount,
                    Description = tr.Description,
                    UserName = tr.User.UserName
                }));

        public Task<IQueryable<TransactionUserListModel>> GetAllByUserIdAsync(string id)
            => Task.FromResult(this.Context
                .Transactions
                .Where(t => t.UserId == id &&
                    (t.Type == TransactionType.Deposit.ToString() ||
                    t.Type == TransactionType.Withdrawal.ToString()))
                .Select(tr => new TransactionUserListModel
                {
                    Date = tr.Date,
                    Type = tr.Type,
                    QuotedCurrency = tr.QuotedCurrency,
                    QuotedCurrencyAmount = tr.QuotedCurrencyAmount,
                    Description = tr.Description
                }));
    }
}
