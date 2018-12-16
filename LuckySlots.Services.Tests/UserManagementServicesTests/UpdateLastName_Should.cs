namespace LuckySlots.Services.Tests.UserManagementServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Admin;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class UpdateLastName_Should
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
        public async Task Set_TheNew_Name()
        {
            var options = GetDbContextOptions("Set_TheNew_Name");

            var user = new User()
            {
                FirstName = "Acho",
                LastName = "Mizinov"
            };

            var newName = string.Empty;

            // Act
            using (var context = new LuckySlotsDbContext(options))
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq);

                newName = "New Mizinov";
                user = await sut.UpdateLastName(user.Id, newName);
            }

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                Assert.AreEqual(newName, user.LastName);
            }
        }

        [TestMethod]
        public async Task ThrowsException_If_UserDoesntExists()
        {
            var options = GetDbContextOptions("ThrowsException_If_UserDoesntExists");
            var userId = Guid.NewGuid().ToString();

            var user = new User()
            {
                Id = userId,
                FirstName = "Acho",
                LastName = "Mizinov"
            };

            // Act & Assert
            using (var context = new LuckySlotsDbContext(options))
            {
                var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq);

                var newName = "New Mizinov";

                await Assert.ThrowsExceptionAsync<UserDoesntExistsException>(() =>
                sut.UpdateLastName(user.Id, newName));
            }

        }

        [TestMethod]
        public async Task ThrowsException_If_UserIsDeleted()
        {
            var options = GetDbContextOptions("ThrowsException_If_UserIsDeleted");
            var userId = Guid.NewGuid().ToString();

            var user = new User()
            {
                Id = userId,
                FirstName = "Acho",
                LastName = "Mizinov",
                IsDeleted = true
            };

            using (var arrangeContext = new LuckySlotsDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new LuckySlotsDbContext(options))
            {
                var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq);

                var newName = "New Mizinov";

                await Assert.ThrowsExceptionAsync<UserDoesntExistsException>(() =>
                sut.UpdateLastName(user.Id, newName));
            }
        }

        [TestMethod]
        public async Task ThrowsException_If_NameIsNull()
        {
            var options = GetDbContextOptions("ThrowsException_If_NameIsNull");
            var userId = Guid.NewGuid().ToString();

            var user = new User()
            {
                Id = userId,
                FirstName = "Acho",
                LastName = "Mizinov"
            };

            // Act & Assert
            using (var context = new LuckySlotsDbContext(options))
            {
                var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                sut.UpdateLastName(user.Id, null));
            }

        }
    }
}
