namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using System.Linq;

    public interface IUserManagementServices
    {
        IQueryable<User> GetAllUsers();
    }
}
