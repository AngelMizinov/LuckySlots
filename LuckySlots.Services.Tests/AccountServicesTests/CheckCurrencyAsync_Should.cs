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
    public class CheckCurrencyAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
           => new DbContextOptionsBuilder<LuckySlotsDbContext>()
           .UseInMemoryDatabase(databaseName: dbName)
           .Options;

        [TestMethod]
        public async Task Returns_CorrectCurrency()
        {
            var options = GetDbContextOptions("Returns_CorrectCurrency");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 450,
                Currency = "BGN"
            };

            string expectedCurrency;
            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();

                var sut = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);
                expectedCurrency = await sut.CheckCurrencyAsync(user.Id);
            }

            Assert.AreEqual(expectedCurrency, user.Currency);
        }

        [TestMethod]
        public async Task Returns_Null_IfUserDoesntExists()
        {
            var options = GetDbContextOptions("Returns_Null_IfUserDoesntExists");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                Currency = "BGN"
            };

            string expectedCurrency;
            using (var actContext = new LuckySlotsDbContext(options))
            {
                var sut = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);
                expectedCurrency = await sut.CheckCurrencyAsync(user.Id);
            }

            Assert.AreEqual(expectedCurrency, null);
        }
    }
}
