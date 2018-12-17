namespace LuckySlots.Services.Tests.GameServicesTests
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
    public class GetNumberOfGamesPlayedByGame_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        [TestMethod]
        public async Task Returns_ProperCount_OfGames()
        {
            var options = GetDbContextOptions("Returns_ProperCount_OfGames");

            var gameName = "gameofcodes";

            var firstTransaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = "Stake",
                Amount = 10,
                BaseCurrency = "USD",
                Description = "Stake on gameofcodes"
            };


            var secondTransaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = "Stake",
                Amount = 15,
                BaseCurrency = "USD",
                Description = "Stake on gameofcodes"
            };


            var thirdTransaction = new Transaction
            {
                Date = DateTime.UtcNow,
                Type = "Win",
                Amount = 12,
                BaseCurrency = "USD",
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

                var result = sut.GetNumberOfGamesPlayedByGame(gameName);

                Assert.AreEqual(2, result);
            }


        }
    }
}
