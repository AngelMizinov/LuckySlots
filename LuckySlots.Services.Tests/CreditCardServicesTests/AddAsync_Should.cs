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
        public async Task CreateCreditCard_When_IsInvoked()
        {
            var options = GetDbContextOptions("CreateCreditCard_When_IsInvoked");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4444",
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2019, 5, 1)
            };

            var cardFromService = new CreditCard();
            using (var actContext = new LuckySlotsDbContext(options))
            {
                var creditCardService = new CreditCardService(actContext);
                cardFromService = await creditCardService.AddAsync(card.Number, card.CVV, card.UserId, card.Expiry);
            }

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                Assert.IsInstanceOfType(cardFromService, typeof(CreditCard));
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

            var cardFromService = new CreditCard();
            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.CreditCards.AddAsync(card);
                await actContext.SaveChangesAsync();
            }

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var creditCardService = new CreditCardService(assertContext);

                await Assert.ThrowsExceptionAsync<CreditCardAlreadyExistsException>(() =>
                creditCardService.AddAsync(card.Number, card.CVV, card.UserId, card.Expiry));
            }
        }

    }
}
