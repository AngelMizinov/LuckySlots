namespace LuckySlots.Services.Tests.GameStatsServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class GetStakedAmountByGame_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        [TestMethod]
        public async Task Returns_ProperSum_OfStakedGames()
        {
            var options = GetDbContextOptions("Returns_ProperSum_OfStakedGames");

            var gameName = "gameofcodes";

            var firstTransaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = "Stake",
                BaseCurrency = "USD",
                BaseCurrencyAmount = 12,
                Description = "Stake on gameofcodes"
            };

            var secondTransaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = "Stake",
                BaseCurrency = "USD",
                BaseCurrencyAmount = 24,
                Description = "Stake on gameofcodes"
            };

            var thirdTransaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = "Win",
                BaseCurrency = "USD",
                BaseCurrencyAmount = 12,
                Description = "Win on gameofcodes"
            };

            using (var actContext = new LuckySlotsDbContext(options))
            {
                await actContext.Transactions.AddAsync(firstTransaction);
                await actContext.Transactions.AddAsync(secondTransaction);
                await actContext.Transactions.AddAsync(thirdTransaction);
                await actContext.SaveChangesAsync();
            }

            using (var assertContext = new LuckySlotsDbContext(options))
            {
                var sut = new GameStatsService(assertContext);

                var result = sut.GetStakedAmountByGame(gameName);

                Assert.AreEqual(36, result);
            }


        }
    }
}
