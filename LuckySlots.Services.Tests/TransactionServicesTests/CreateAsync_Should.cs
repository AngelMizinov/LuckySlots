namespace LuckySlots.Services.Tests.TransactionServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using LuckySlots.Services.Transactions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class CreateAsync_Should
    {
        [TestMethod]
        public async Task CreateAnInstance_WhenInvoked()
        {
            // Arrange & Act
            var dbName = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            using (var arrangeDbContext = new LuckySlotsDbContext(GetDbContextOptions(dbName)))
            {
                var userStore = new UserStore<User>(arrangeDbContext);
                var userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);

                var user = new User
                {
                    Id = userId
                };

                await arrangeDbContext.Users.AddAsync(user);
                await arrangeDbContext.SaveChangesAsync();

                var sut = new TransactionServices(arrangeDbContext, userManager);
                await sut.CreateAsync(userId, TransactionType.Deposit, 100m, "Valid desctiption");
            }

            // Assert
            using (var assertDbContext = new LuckySlotsDbContext(GetDbContextOptions(dbName)))
            {
                Assert.IsNotNull(assertDbContext.Transactions.Where(t => t.UserId.ToString() == userId));
            }
        }

        [TestMethod]
        public async Task ThrowsException_When_UserDoesntExists()
        {
            // Arrange & Act & Assert
            var dbName = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            using (var context = new LuckySlotsDbContext(GetDbContextOptions(dbName)))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);

                var user = new User
                {
                    Id = userId
                };
                
                var sut = new TransactionServices(context, userManager);
               
                await Assert.ThrowsExceptionAsync<UserDoesntExistsException>(() =>
                    sut.CreateAsync(userId, TransactionType.Deposit, 100m, "Valid desctiption"));
            }
        }

        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbContextName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
                .UseInMemoryDatabase(dbContextName)
                .Options;
    }
}
