namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITransactionServices
    {

        Task<IQueryable<Transaction>> GetAllAsync();

        Task<IQueryable<Transaction>> GetAllByUserIdAsync(string id);

        Task<Transaction> CreateAsync(string userId, TransactionType type, decimal amount, string description);
    }
}
