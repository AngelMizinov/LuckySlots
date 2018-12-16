namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Infrastructure.Enums;
    using System.Threading.Tasks;

    public interface IAccountService
    {
        Task<decimal> DepositAsync(string userId, decimal amount, TransactionType type, string cardId = null, string gameName = null);

        Task<decimal> CheckBalanceAsync(string userId);

        Task<string> CheckCurrencyAsync(string userId);

        Task<decimal> ChargeAccountAsync(string userId, decimal amount, TransactionType type, string gameName = null);
    }
}
