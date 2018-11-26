using LuckySlots.Data;
using LuckySlots.Data.Models;
using LuckySlots.Services.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySlots.Services.Tests.AccountServicesTests
{
    [TestClass]
    public class CheckBalanceAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
           => new DbContextOptionsBuilder<LuckySlotsDbContext>()
           .UseInMemoryDatabase(databaseName: dbName)
           .Options;


        [TestMethod]
        public async Task Return_AccountBalance()
        {
            var options = GetDbContextOptions("Return_AccountBalance");

            var user = new User()
            {
                Id = "1",
                AccountBalance = 450
            };

            decimal expectedBalance;

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();

                var accountService = new AccountService(actContext);
                expectedBalance = await accountService.CheckBalanceAsync(user.Id);
            }

            using(var assertContext = new LuckySlotsDbContext(options))
            {
                Assert.AreEqual(expectedBalance, user.AccountBalance);
            }

        }
    }
}
