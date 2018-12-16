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

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            mockJsonParser
                .Setup(jp => jp.ExtractExchangeRate(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<double>());

            decimal expectedBalance;
            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();

                var sut = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object
                    , mockJsonParser.Object);
                expectedBalance = await sut.ChargeAccountAsync(user.Id, 200, TransactionType.Stake);
            }

            var newBalance = user.AccountBalance;

            Assert.AreEqual(expectedBalance, newBalance);
        }

        [TestMethod]
        public async Task ThrowsException_When_UserBalanceIsNotEnough_ForCharging()
        {
            var options = GetDbContextOptions("ThrowsException_When_UserBalanceIsNotEnough_ForCharging");

            var mockTransactionServices = new Mock<ITransactionServices>();
            var mockCreditCardServices = new Mock<ICreditCardService>();
            var mockJsonParser = new Mock<IJsonParser>();

            var user = new User()
            {
                Id = "1",
                AccountBalance = 300
            };

            mockJsonParser
              .Setup(jp => jp.ExtractExchangeRate(It.IsAny<string>()))
              .ReturnsAsync(It.IsAny<double>());

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(user);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new AccountService(assertContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);

                await Assert.ThrowsExceptionAsync<InsufficientFundsException>(() =>
                    sut.ChargeAccountAsync(user.Id, 500, TransactionType.Stake));
            }
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

            mockJsonParser
              .Setup(jp => jp.ExtractExchangeRate(It.IsAny<string>()))
              .ReturnsAsync(It.IsAny<double>());

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new AccountService(assertContext, mockTransactionServices.Object, mockCreditCardServices.Object,
                    mockJsonParser.Object);

                await Assert.ThrowsExceptionAsync<UserDoesntExistsException>(() =>
                    sut.ChargeAccountAsync(user.Id, 250, TransactionType.Stake));
            }
        }

    }
}