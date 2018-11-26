﻿namespace LuckySlots.Services.Tests.AccountServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Account;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ChargeAccountAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        [TestMethod]
        public async Task ReturnNewBalance_When_AccountIsCharged()
        {
            var options = GetDbContextOptions("ReturnNewBalance_When_AccountIsCharged");

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
                expectedBalance = await accountService.ChargeAccountAsync(user.Id, 200);
            }

            var newBalance = user.AccountBalance;

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                Assert.AreEqual(expectedBalance, newBalance);
            }
        }

        [TestMethod]
        public async Task ThrowsArgumentException_When_UserBalanceIsNotEnough_ForCharging()
        {
            var options = GetDbContextOptions("ThrowsArgumentException_When_UserBalanceIsNotEnough_ForCharging");

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var accountService = new AccountService(assertContext);

                await Assert.ThrowsExceptionAsync<InsufficientFundsException>(() =>
                accountService.ChargeAccountAsync(user.Id, 500));
            }
        }
    }
}
