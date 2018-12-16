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
    using LuckySlots.Services.Infrastructure.Exceptions;

    [TestClass]
    public class GetLastForDigitsOfCardNumberAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
        => new DbContextOptionsBuilder<LuckySlotsDbContext>()
       .UseInMemoryDatabase(databaseName: dbName)
       .Options;

        [TestMethod]
        public async Task Returns_Only_LastFourDigitsOfCard()
        {
            var options = GetDbContextOptions("Returns_Only_LastFourDigitsOfCard");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4566",
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2020, 11, 1)
            };

            var cardNumber = string.Empty;
            // Act
            using (var actContext = new LuckySlotsDbContext(options))
            {
                var addedCard = await actContext.CreditCards.AddAsync(card);
                await actContext.SaveChangesAsync();

                var addedCardId = addedCard.Entity.Id.ToString();

                var sut = new CreditCardService(actContext);
                cardNumber = await sut.GetLastForDigitsOfCardNumberAsync(addedCardId);
            }

            Assert.IsTrue(cardNumber == card.Number.Substring(card.Number.Length - 4));
        }

        [TestMethod]
        public async Task ThrowsException_IfCardId_IsNull()
        {
            var options = GetDbContextOptions("ThrowsException_IfCardId_IsNull");
            var userId = Guid.NewGuid().ToString();

            List<CreditCard> cards = new List<CreditCard>();

            for (int i = 0; i < 2; i++)
            {
                cards.Add(new CreditCard()
                {
                    Number = $"1111 2222 3333 744{i}",
                    CVV = (128 + i),
                    UserId = userId,
                    Expiry = new DateTime(2020, 12, 1 + i)
                });
            }
            
            using (var actContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(actContext);

                await Assert.ThrowsExceptionAsync<CreditCardDoesntExistsException>(() =>
                    sut.GetLastForDigitsOfCardNumberAsync(null));
            }
        }
    }
}
