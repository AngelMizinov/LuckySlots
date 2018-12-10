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

    [TestClass]
    public class GetAllByUserIdAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
        => new DbContextOptionsBuilder<LuckySlotsDbContext>()
        .UseInMemoryDatabase(databaseName: dbName)
        .Options;

        [TestMethod]
        public async Task ReturnsOnly_User_Cards()
        {
            var options = GetDbContextOptions("ReturnsOnly_User_Cards");
            var firstUserId = Guid.NewGuid().ToString();
            var secondUserId = Guid.NewGuid().ToString();

            List<CreditCard> cards = new List<CreditCard>();

            // Add first user cards
            for (int i = 0; i < 2; i++)
            {
                cards.Add(new CreditCard()
                {
                    Number = $"1111 2222 3333 744{i}",
                    CVV = (128 + i),
                    UserId = firstUserId,
                    Expiry = new DateTime(2020, 12, 1 + i)
                });
            }

            // Add second user cards
            for (int i = 0; i < 2; i++)
            {
                cards.Add(new CreditCard()
                {
                    Number = $"1111 2222 3333 744{i + 2}",
                    CVV = (128 + i + 2),
                    UserId = secondUserId,
                    Expiry = new DateTime(2019, 8, 1 + i + 1)
                });
            }

            // Act
            using (var actContext = new LuckySlotsDbContext(options))
            {
                foreach (var card in cards)
                {
                    await actContext.CreditCards.AddAsync(card);
                }
                await actContext.SaveChangesAsync();
            }

            // Assert
            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var creditCardService = new CreditCardService(assertContext);
                var cardsNumbers = await creditCardService.GetAllByUserIdAsync(firstUserId);

                Assert.IsTrue(2 == cardsNumbers.Count);
            }

        }
    }
}
