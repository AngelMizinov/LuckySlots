namespace LuckySlots.Services.Tests.UserManagementServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Admin;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class GetAllUsersAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;


        private static IRoleStore<IdentityRole> roleStoreMoq = new Mock<IRoleStore<IdentityRole>>().Object;
        private static RoleManager<IdentityRole> roleManagerMoq = new RoleManager<IdentityRole>(roleStoreMoq, null, null, null, null);

        private static IUserStore<User> userStoreMoq = new Mock<IUserStore<User>>().Object;
        private static UserManager<User> userManagerMoq = new UserManager<User>(userStoreMoq, null, null, null, null, null, null, null, null);

        [TestMethod]
        public async Task Returns_AllUsers()
        {
            var options = GetDbContextOptions("Returns_AllUsers");

            List<User> users = new List<User>();

            for (int i = 0; i < 10; i++)
            {
                var user = new User()
                {
                    FirstName = $"FirstName{i + 1}",
                    LastName = $"LastName{i + 1}",
                    Email = $"user@email{i + 1}"
                };

                users.Add(user);
            }

            // Act & Assert
            using (var context = new LuckySlotsDbContext(options))
            {
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();

                var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq);

                var allUsers = await sut.GetAllUsersAsync();
                var allUsersAsList = allUsers.ToList();

                Assert.AreEqual(10, allUsersAsList.Count);
            }
        }

        [TestMethod]
        public async Task Returns_EmptyCollection_If_ThereAreNoUsers()
        {
            var options = GetDbContextOptions("Returns_EmptyCollection_If_ThereAreNoUsers");
            
            // Act & Assert
            using (var context = new LuckySlotsDbContext(options))
            {
                var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq);

                var allUsers = await sut.GetAllUsersAsync();
                var allUsersAsList = allUsers.ToList();

                Assert.AreEqual(0, allUsersAsList.Count);
            }
        }

    }
}
