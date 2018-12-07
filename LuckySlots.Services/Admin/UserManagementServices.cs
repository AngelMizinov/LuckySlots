namespace LuckySlots.Services.Admin
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserManagementServices : BaseService, IUserManagementServices
    {
        public UserManagementServices(LuckySlotsDbContext context)
            : base(context)
        {
        }

        public IQueryable<User> GetAllUsers()
        {
            return this.Context.Users.AsQueryable();
        }

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

    }
}
