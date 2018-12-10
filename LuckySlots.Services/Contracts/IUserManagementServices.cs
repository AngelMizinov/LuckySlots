namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUserManagementServices
    {
        IQueryable<User> GetAllUsers();

        Task<User> UpdateFirstName(string userId, string name);

        Task<User> UpdateLastName(string userId, string name);
        object GetUserIdAsync(ClaimsPrincipal user);
    }
}
