//namespace LuckySlots.Services.Tests.AccountServicesTests
//{
//    using LuckySlots.Data;
//    using LuckySlots.Data.Models;
//    using LuckySlots.Infrastructure.Enums;
//    using LuckySlots.Services.Account;
//    using LuckySlots.Services.Contracts;
//    using LuckySlots.Services.Infrastructure.Exceptions;
//    using Microsoft.EntityFrameworkCore;
//    using Microsoft.VisualStudio.TestTools.UnitTesting;
//    using Moq;
//    using System;
//    using System.Collections.Generic;
//    using System.Text;
//    using System.Threading.Tasks;

//    [TestClass]
//    public class ChargeAccountAsync_Should
//    {
//        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
//            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
//            .UseInMemoryDatabase(databaseName: dbName)
//            .Options;

//        [TestMethod]
//        public async Task ReturnNewBalance_When_AccountIsCharged()
//        {
//            var options = GetDbContextOptions("ReturnNewBalance_When_AccountIsCharged");

//            var mockTransactionServices = new Mock<ITransactionServices>();
//            var mockCreditCardServices = new Mock<ICreditCardService>();

//            var user = new User()
//            {
//                Id = "1",
//                AccountBalance = 300
//            };

//            decimal expectedBalance;

//            using (var actContext = new LuckySlotsDbContext(options))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.SaveChangesAsync();

//                var accountService = new AccountService(actContext, mockTransactionServices.Object, mockCreditCardServices.Object);
//                expectedBalance = await accountService.ChargeAccountAsync(user.Id, 200, TransactionType.Stake);
//            }

//            var newBalance = user.AccountBalance;

//            using (var assertContext = new LuckySlotsDbContext(options))
//            {
//                Assert.AreEqual(expectedBalance, newBalance);
//            }
//        }

//        [TestMethod]
//        public async Task ThrowsArgumentException_When_UserBalanceIsNotEnough_ForCharging()
//        {
//            var options = GetDbContextOptions("ThrowsArgumentException_When_UserBalanceIsNotEnough_ForCharging");

//            var mockTransactionServices = new Mock<ITransactionServices>();
//            var mockCreditCardServices = new Mock<ICreditCardService>();

//            var user = new User()
//            {
//                Id = "1",
//                AccountBalance = 300
//            };

//            using (var actContext = new LuckySlotsDbContext(options))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.SaveChangesAsync();
//            }

//            using (var assertContext = new LuckySlotsDbContext(options))
//            {
//                var accountService = new AccountService(assertContext, mockTransactionServices.Object, mockCreditCardServices.Object);

//                await Assert.ThrowsExceptionAsync<InsufficientFundsException>(() =>
//                accountService.ChargeAccountAsync(user.Id, 500, TransactionType.Stake));
//            }
//        }

//        [TestMethod]
//        public async Task CreateNewTransaction_When_IsExecuted()
//        {
//            var options = GetDbContextOptions("CreateNewTransaction_When_IsExecuted");

//            var mockTransactionServices = new Mock<ITransactionServices>();
//            var mockCreditCardServices = new Mock<ICreditCardService>();

//            var user = new User()
//            {
//                Id = "1",
//                AccountBalance = 300
//            };
            
//            using (var actContext = new LuckySlotsDbContext(options))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.SaveChangesAsync();
//            }

//            using (var assertContext = new LuckySlotsDbContext(options))
//            {
//                var accountService = new AccountService(assertContext, mockTransactionServices.Object, mockCreditCardServices.Object);
//                var expectedBalance = await accountService.ChargeAccountAsync(user.Id, 200, TransactionType.Stake);

//                mockTransactionServices.Verify(transServices => transServices.CreateAsync(It.IsAny<string>(), It.IsAny<TransactionType>(),
//                    It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
//            }

//        }
//    }
//}
