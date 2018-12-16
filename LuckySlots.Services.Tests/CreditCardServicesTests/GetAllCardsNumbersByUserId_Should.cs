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

    [TestClass]
    public class GetAllCardsNumbersByUserId_Should
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
                var sut = new CreditCardService(assertContext);
                var cardsNumbers = await sut.GetAllCardsNumbersByUserId(firstUserId);

                Assert.IsTrue(cardsNumbers.Count == 2);
            }
        }

        [TestMethod]
        public async Task ReturnsOnly_LastFourDigits()
        {
            var options = GetDbContextOptions("ReturnsOnly_LastFourDigits");
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
                var sut = new CreditCardService(assertContext);
                var cardsNumbers = await sut.GetAllCardsNumbersByUserId(firstUserId);
                var cardsNumbersToList = cardsNumbers.ToList();

                var firstCardNumber = cards[0].Number.Substring(cards[0].Number.Length - 4);
                var secondCardNumber = cards[1].Number.Substring(cards[1].Number.Length - 4);

                Assert.AreEqual(cardsNumbersToList[0], firstCardNumber);
                Assert.AreEqual(cardsNumbersToList[1], secondCardNumber);
            }
        }

        [TestMethod]
        public async Task Returns_EmptyCollection_IfUserDoesntExists()
        {
            var options = GetDbContextOptions("Returns_EmptyCollection_IfUserDoesntExists");
            var userId = Guid.NewGuid().ToString();
            var wrongUserId = Guid.NewGuid().ToString();

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

            var user = new User()
            {
                Id = wrongUserId,
                Currency = "BGN"
            };

            using (var context = new LuckySlotsDbContext(options))
            {
                foreach (var card in cards)
                {
                    await context.CreditCards.AddAsync(card);
                }

                var sut = new CreditCardService(context);

                var result = await sut.GetAllCardsNumbersByUserId(user.Id);

                Assert.AreEqual(result.Count, 0);
            }
        }
    }
}
