namespace LuckySlots.Services.Tests.CreditCardServicesTests
{
    using LuckySlots.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.CreditCard;
    using System.Linq;
    using LuckySlots.Services.Infrastructure.Exceptions;

    [TestClass]
    public class AddAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        [TestMethod]
        public async Task CreateCreditCard_When_IsCalled()
        {
            var options = GetDbContextOptions("CreateCreditCard_When_IsCalled");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4444",
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2019, 5, 1)
            };

            using (var actContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(actContext);
                await sut.AddAsync(card.Number, card.CVV, card.UserId, card.Expiry);
            }

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var cardsNumber = await assertContext.CreditCards.CountAsync();
                Assert.IsTrue(cardsNumber == 1);
            }
        }

        [TestMethod]
        public async Task ThrowsException_When_CardAlreadyExists()
        {
            var options = GetDbContextOptions("ThrowsException_When_CardAlreadyExists");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4444",
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2019, 5, 1)
            };

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.CreditCards.AddAsync(card);
                await actContext.SaveChangesAsync();
            }

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(assertContext);

                await Assert.ThrowsExceptionAsync<CreditCardAlreadyExistsException>(() =>
                    sut.AddAsync(card.Number, card.CVV, card.UserId, card.Expiry));
            }
        }

        [TestMethod]
        public async Task ThrowsException_When_CardNumberIsNull()
        {
            var options = GetDbContextOptions("ThrowsException_When_CardNumberIsNull");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = null,
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2019, 5, 1)
            };
            
            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(assertContext);

                await Assert.ThrowsExceptionAsync<CreditCardDoesntExistsException>(() =>
                    sut.AddAsync(card.Number, card.CVV, card.UserId, card.Expiry));
            }
        }

        [TestMethod]
        public async Task ThrowsException_When_UserIdIsNull()
        {
            var options = GetDbContextOptions("ThrowsException_When_UserIdIsNull");
            
            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4444",
                CVV = 123,
                UserId = null,
                Expiry = new DateTime(2019, 5, 1)
            };

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(assertContext);

                await Assert.ThrowsExceptionAsync<UserDoesntExistsException>(() =>
                    sut.AddAsync(card.Number, card.CVV, card.UserId, card.Expiry));
            }
        }

    }
}
