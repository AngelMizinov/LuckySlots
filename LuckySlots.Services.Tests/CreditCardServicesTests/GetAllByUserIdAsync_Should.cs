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

            var firstUser = new User()
            {
                 Id = firstUserId
            };

            var secondtUser = new User()
            {
                Id = secondUserId
            };

            List<CreditCard> cards = new List<CreditCard>();

            // Add first user cards
            for (int i = 0; i < 2; i++)
            {
                cards.Add(new CreditCard()
                {
                    Number = $"1111 2222 3333 744{i}",
                    CVV = (128 + i),
                    UserId = firstUser.Id,
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
                    UserId = secondtUser.Id,
                    Expiry = new DateTime(2019, 8, 1 + i + 1)
                });
            }

            // Act
            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Users.AddAsync(firstUser);
                await actContext.Users.AddAsync(secondtUser);
                
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
                var cardsNumbers = await creditCardService.GetAllByUserIdAsync(firstUser.Id);

                Assert.IsTrue(cardsNumbers.Count == 2);
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

            using (var actContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(actContext);

                var result = await sut.GetAllByUserIdAsync(user.Id);

                Assert.AreEqual(result.Count, 0);
            }
        }
    }
}
