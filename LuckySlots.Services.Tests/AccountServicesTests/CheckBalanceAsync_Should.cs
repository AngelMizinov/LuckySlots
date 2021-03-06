namespace LuckySlots.Services.Tests.AccountServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Providers;
    using LuckySlots.Services.Account;
    using LuckySlots.Services.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class CheckBalanceAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
           => new DbContextOptionsBuilder<LuckySlotsDbContext>()
           .UseInMemoryDatabase(databaseName: dbName)
           .Options;
        
        [TestMethod]
        public async Task Returns_AccountBalance()
        {
            var options = GetDbContextOptions("Returns_AccountBalance");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

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

                var sut = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);
                expectedBalance = await sut.CheckBalanceAsync(user.Id);
            }

            Assert.AreEqual(expectedBalance, user.AccountBalance);
        }

        [TestMethod]
        public async Task Returns_Zero_IfUserDoesntExists()
        {
            var options = GetDbContextOptions("Returns_Null_IfUserDoesntExists");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 450
            };

            decimal expectedBalance;
            using (var actContext = new LuckySlotsDbContext(options))
            {
                var sut = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);
                expectedBalance = await sut.CheckBalanceAsync(user.Id);
            }

            Assert.AreEqual(expectedBalance, 0);
        }
    }
}