namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionServices
    {

        Task<IEnumerable<Transaction>> GetAllAsync();

        Task<IEnumerable<Transaction>> GetAllByIdAsync(string id);

        Task<Transaction> CreateAsync(string userId, TransactionType type, decimal amount, string description);
    }
}
