namespace LuckySlots.Services.Tests.TransactionServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Transactions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class GetAllAsync_Should
    {
        [TestMethod]
        public async Task ReturnAllTransactions_WhenInvoked()
        {
            // Arrange
            var userStoreMoq = new Mock<IUserStore<User>>().Object;
            var userManagerMoq = new UserManager<User>(userStoreMoq, null, null, null, null, null, null, null, null);
            var transactions = new List<Transaction>();
            var dbContextName = Guid.NewGuid().ToString();

            using (var dbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName)))
            {
                for (int i = 0; i < 10; i++)
                {
                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        Amount = i + 1 * 100
                    };

                    transactions.Add(transaction);
                }

                await dbContext.AddRangeAsync(transactions);
                await dbContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertDbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName)))
            {
                var sut = new TransactionServices(assertDbContext, userManagerMoq);
                var expected = 10;
                var result = await sut.GetAllAsync();

                Assert.IsTrue(expected == result.Count());
            }
        }

        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbContextName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
                .UseInMemoryDatabase(dbContextName)
                .Options;
    }
}
