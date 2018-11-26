namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICreditCardService
    {
        Task<CreditCard> GetCreditCardByIdAsync(string id);

        Task<CreditCard> AddAsync(string number, int cvv, string userId, DateTime expiry);

        Task<ICollection<CreditCard>> GetAllByUserIdAsync(string userId);

        Task DeleteAsync(string id);
    }
}
