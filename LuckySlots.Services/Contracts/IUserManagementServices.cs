namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserManagementServices
    {
        Task<User> GetUserByIdAsync(string userId);

        Task<IQueryable<UserListViewModel>> GetAllUsersAsync();

<<<<<<< HEAD
        Task<bool> ToggleRole(User user, string role);
=======
        Task<EntityEntry<IdentityUserRole<string>>> ToggleRole(User user, string roleName);
>>>>>>> 418ef0cc9cadc470cbb68e77f0d92295afd438f2

        Task<User> UpdateFirstName(string userId, string name);

        Task<User> UpdateLastName(string userId, string name);

        Task<bool> ToggleUserLock(string userId);
    }
}
