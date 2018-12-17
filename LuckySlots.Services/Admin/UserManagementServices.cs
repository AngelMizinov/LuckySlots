namespace LuckySlots.Services.Admin
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using LuckySlots.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserManagementServices : BaseService, IUserManagementServices
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagementServices(
            LuckySlotsDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public Task<IQueryable<UserListViewModel>> GetAllUsersAsync()
            => Task.FromResult(this.Context
                .Users.Select(u => new UserListViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    IsSupport = u.IsSupport,
                    IsAccountLocked = u.IsAccountLocked
                }));

        //public async Task<bool> ToggleRole(User user, string role);
        public async Task<IdentityResult> ToggleRole(string userName, string roleName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            // Check if user exists
            if (user == null)
            {
                throw new NullReferenceException("User cannot be null.");
            }

            // Check if role exists
            var roleExists = await this.roleManager.RoleExistsAsync(roleName);

            //var existingRole = await this.Context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);

            if (!roleExists)
            {
                throw new ArgumentException("Role does not exist.");
            }

            var isInRole = await this.userManager.IsInRoleAsync(user, roleName);
            var result = new IdentityResult();

            if (isInRole == true)
            {
                result = await this.userManager.RemoveFromRoleAsync(user, roleName);

                if (roleName == GlobalConstants.AdministratorRoleName)
                {
                    user.IsAdmin = false;
                }
                else
                {
                    user.IsSupport = false;
                }
            }
            else
            {
                result = await this.userManager.AddToRoleAsync(user, roleName);

                if (roleName == GlobalConstants.AdministratorRoleName)
                {
                    user.IsAdmin = true;
                }
                else
                {
                    user.IsSupport = true;
                }
            }

            await this.Context.SaveChangesAsync();

            return result;
        }

        public async Task<User> UpdateFirstName(string userId, string name)
        {
            if (name == null)
            {
                throw new ArgumentException("Name cannot be null.");
            }

            var user = await this.Context.Users
                .Where(us => us.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null || user.IsDeleted == true)
            {
                throw new UserDoesntExistsException("User with this id does not exists.");
            }

            user.FirstName = name;

            await this.Context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateLastName(string userId, string name)
        {
            if (name == null)
            {
                throw new ArgumentException("Name cannot be null.");
            }

            var user = await this.Context.Users
                .Where(us => us.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null || user.IsDeleted == true)
            {
                throw new UserDoesntExistsException("User with this id does not exists.");
            }

            user.LastName = name;

            await this.Context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ToggleUserLock(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User does not exist.");
            }

            user.IsAccountLocked = !user.IsAccountLocked;

            await this.Context.SaveChangesAsync();

            return user.IsAccountLocked;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User does not exits.");
            }

            return user;
        }
    }
}
