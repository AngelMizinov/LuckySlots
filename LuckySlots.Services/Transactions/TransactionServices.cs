namespace LuckySlots.Services.Transactions
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
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
            this.userManager = userManager;
        }

        public async Task<Transaction> CreateAsync(string userId, string type, decimal amount, string description)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                User = user,
                Type = type,
                Amount = amount,
                Description = description
            };

            await this.Context.Transactions.AddAsync(transaction);

            await this.Context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
            => await Task.Run(() => this.Context.Transactions);


        public async Task<IEnumerable<Transaction>> GetAllByIdAsync(string id)
            => await Task.Run(() => this.Context
                .Transactions
                .Where(t => t.UserId == id)
                .ToList());
    }
}
