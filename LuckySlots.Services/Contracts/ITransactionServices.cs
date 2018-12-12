namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITransactionServices
    {

        Task<IQueryable<TransactionAdminListModel>> GetAllAsync();

        Task<IQueryable<TransactionUserListModel>> GetAllByUserIdAsync(string id);

        Task<Transaction> CreateAsync(string userId, TransactionType type, decimal amount, string description);
    }
}
