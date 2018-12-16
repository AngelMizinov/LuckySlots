namespace LuckySlots.Services.Tests.AccountServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Infrastructure.Providers;
    using LuckySlots.Services.Account;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
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

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            double exchangeRate = 1.56;

            mockJsonParser
                .Setup(jp => jp.ExtractExchangeRate(It.IsAny<string>()))
                .ReturnsAsync(exchangeRate);

            decimal expectedBalance;
            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();

                var sut = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);
                expectedBalance = await sut.DepositAsync(user.Id, 250, TransactionType.Deposit);
            }

            var newBalance = user.AccountBalance;

            Assert.AreEqual(expectedBalance, newBalance);
        }

        [TestMethod]
        public async Task CreateTransaction_When_IsExecuted()
        {
            var options = GetDbContextOptions("CreateTransaction_When_IsExecuted");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            double exchangeRate = 1.56;

            mockJsonParser
                .Setup(jp => jp.ExtractExchangeRate(It.IsAny<string>()))
                .ReturnsAsync(exchangeRate);

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new AccountService(assertContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);
                var expectedBalance = await sut.ChargeAccountAsync(user.Id, 200, TransactionType.Stake);

                var transactionsNum = await assertContext.Transactions.CountAsync();

                Assert.IsTrue(transactionsNum == 1);
            }
        }

        // TODO: Test if throws TransactionFailedException
        //[TestMethod]
        public void ThrowsException_When_TrasanctionIsNotAdded()
        {

        }

        [TestMethod]
        public async Task ThrowsException_When_UserDoesntExists()
        {
            var options = GetDbContextOptions("ThrowsException_When_UserDoesntExists");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            double exchangeRate = 1.56;

            mockJsonParser
                .Setup(jp => jp.ExtractExchangeRate(It.IsAny<string>()))
                .ReturnsAsync(exchangeRate);

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new AccountService(assertContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);

                await Assert.ThrowsExceptionAsync<UserDoesntExistsException>(() =>
                    sut.ChargeAccountAsync(user.Id, 200, TransactionType.Stake));

            }
        }


    }
}