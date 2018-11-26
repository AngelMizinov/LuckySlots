namespace LuckySlots.Services.Tests.AccountServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Account;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class DepositAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
          => new DbContextOptionsBuilder<LuckySlotsDbContext>()
          .UseInMemoryDatabase(databaseName: dbName)
          .Options;
        
        [TestMethod]
        public async Task ReturnsNewBalance_When_DepositIsRealized()
        {
            var options = GetDbContextOptions("ReturnsNewBalance_When_DepositIsRealized");

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            decimal expectedBalance;

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();

                var accountService = new AccountService(actContext);
                expectedBalance = await accountService.DepositAsync(user.Id,250);
            }

            var newBalance = user.AccountBalance;

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                Assert.AreEqual(expectedBalance, newBalance);
            }
        }
    }
}
