namespace LuckySlots.Services.Tests.TransactionServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
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
    public class GetAllByUserIdAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectTransactions_WhenInvokedWithUserId()
        {
            // Arrange
            var userStoreMoq = new Mock<IUserStore<User>>().Object;
            var userManagerMoq = new UserManager<User>(userStoreMoq, null, null, null, null, null, null, null, null);
            var transactions = new List<Transaction>();
            var dbContextName = Guid.NewGuid().ToString();
            var incorrectUserId = Guid.NewGuid().ToString();

            using (var arrangeDbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName)))
            {
                var user = new User()
                {
                    FirstName = "Angel",
                    LastName = "Mizinov"
                };

                await arrangeDbContext.Users.AddAsync(user);
                await arrangeDbContext.SaveChangesAsync();

                var correctUserId = user.Id;

                for (int i = 0; i < 10; i++)
                {
                    var id = (i & 1) == 0 ? correctUserId : incorrectUserId;

                    var transaction = new Transaction
                    {
                        UserId = id.ToString(),
                        Amount = i + 1 * 100,
                        Type = TransactionType.Deposit.ToString()
                    };

                    transactions.Add(transaction);
                }

                await arrangeDbContext.AddRangeAsync(transactions);
                await arrangeDbContext.SaveChangesAsync();

                var sut = new TransactionServices(arrangeDbContext, userManagerMoq);
                var expected = 5;
                var result = await sut.GetAllByUserIdAsync(correctUserId);
                var materializeResult = result.ToList();

                Assert.IsTrue(expected == materializeResult.Count);
            }
        }

        [TestMethod]
        public async Task NotReturnAnyResults_WhenPassedANonEnexistingUserId()
        {
            // Arrange
            var userStoreMoq = new Mock<IUserStore<User>>().Object;
            var userManagerMoq = new UserManager<User>(userStoreMoq, null, null, null, null, null, null, null, null);
            var transactions = new List<Transaction>();
            var dbContextName = Guid.NewGuid().ToString();
            var nonExistingUserId = Guid.NewGuid();

            using (var arrangeDbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName)))
            {
                for (int i = 0; i < 10; i++)
                {
                    var transaction = new Transaction
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Amount = i + 1 * 100
                    };

                    transactions.Add(transaction);
                }

                await arrangeDbContext.AddRangeAsync(transactions);
                await arrangeDbContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertDbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName)))
            {
                var sut = new TransactionServices(assertDbContext, userManagerMoq);
                var exptected = 0;
                var result = await sut.GetAllByUserIdAsync(nonExistingUserId.ToString());

                Assert.IsTrue(exptected == result.Count());
            }
        }

        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbContextName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
                .UseInMemoryDatabase(dbContextName)
                .Options;
    }
}
