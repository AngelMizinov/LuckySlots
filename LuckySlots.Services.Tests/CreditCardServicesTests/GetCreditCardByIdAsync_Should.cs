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
    public class GetCreditCardByIdAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
       => new DbContextOptionsBuilder<LuckySlotsDbContext>()
       .UseInMemoryDatabase(databaseName: dbName)
       .Options;

        [TestMethod]
        public async Task Returns_Null_IfCardDoesntExists()
        {
            var options = GetDbContextOptions("Returns_Null_IfCardDoesntExists");

            var card = new CreditCard()
            {
                Id = Guid.NewGuid(),
                Number = "1111 2222 3333 4444",
                CVV = 123,
                Expiry = new DateTime(2020, 05, 1)
            };

            CreditCard result = new CreditCard();
            using (var actContext = new LuckySlotsDbContext(options))
            {
                var sut = new CreditCardService(actContext);

                result = await sut.GetCreditCardByIdAsync(card.Id.ToString());
            }

            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public async Task Returns_CorrectCard()
        {
            var options = GetDbContextOptions("Returns_CorrectCard");

            var card = new CreditCard()
            {
                Id = Guid.NewGuid(),
                Number = "1111 2222 3333 4444",
                CVV = 123,
                Expiry = new DateTime(2020, 05, 1)
            };

            using (var context = new LuckySlotsDbContext(options))
            {
                await context.CreditCards.AddAsync(card);
                await context.SaveChangesAsync();

                var sut = new CreditCardService(context);

                var result = await sut.GetCreditCardByIdAsync(card.Id.ToString());

                Assert.AreEqual(card.Id, result.Id);
            }
        }

    }
}
