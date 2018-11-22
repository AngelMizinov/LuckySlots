namespace LuckySlots.Services.Admin
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using System.Linq;

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
    }
}
