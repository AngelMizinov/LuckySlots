﻿namespace LuckySlots.Services.Admin
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Security.Claims;
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

        public async Task<bool> ToggleRole(User user, string role)
        {
            try
            {
                if (user == null)
                {
                    throw new NullReferenceException("User cannot be null.");
                }

                var existingRole = await this.roleManager.FindByNameAsync(role);

                if (existingRole == null)
                {
                    throw new ArgumentException("Role does not exist.");
                }

                var isInRole = await this.userManager.IsInRoleAsync(user, role);

                if (isInRole == true)
                {
                    throw new ArgumentException($"User {user.UserName} is already in role {role}.");
                }
            }
            catch (Exception)
            {

                throw;
            }

            await this.userManager.AddToRoleAsync(user, role);
            await this.Context.SaveChangesAsync();

            return user.IsSupport;
        }

        //public async Task<string> GetUserIdAsync(ClaimsPrincipal claimsPrincipal)
        //{
        //    return this.userManager.GetUserId(claimsPrincipal);
        //}

        public async Task<User> UpdateFirstName(string userId, string name)
        {
            var user = await this.Context.Users
                .Where(us => us.Id == userId)
                .FirstOrDefaultAsync();

            user.FirstName = name;

            await this.Context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateLastName(string userId, string name)
        {
            var user = await this.Context.Users
                .Where(us => us.Id == userId)
                .FirstOrDefaultAsync();

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
