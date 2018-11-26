namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionServices
    {

        Task<ICollection<Transaction>> GetAllAsync();

        Task<ICollection<Transaction>> GetAllByUserIdAsync(string id);

        Task<Transaction> CreateAsync(string userId, string type, decimal amount, string description);
    }
}
