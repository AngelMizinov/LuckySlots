namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserManagementServices
    {
        Task<IQueryable<UserListViewModel>> GetAllUsersAsync();

        Task<IdentityResult> ToggleRole(User user, string role);

        Task<User> UpdateFirstName(string userId, string name);

        Task<User> UpdateLastName(string userId, string name);

        //Task<string> GetUserIdAsync(ClaimsPrincipal claimsPrincipal);
    }
}
