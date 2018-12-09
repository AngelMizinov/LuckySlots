namespace LuckySlots.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using LuckySlots.Data.Models;

    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
    }
}
