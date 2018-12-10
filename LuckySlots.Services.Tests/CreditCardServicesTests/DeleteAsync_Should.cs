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
    public class DeleteAsync_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
          => new DbContextOptionsBuilder<LuckySlotsDbContext>()
          .UseInMemoryDatabase(databaseName: dbName)
          .Options;

        [TestMethod]
        public async Task SetInstance_IsDeleted_To_True()
        {
            var options = GetDbContextOptions("SetInstance_IsDeleted_To_True");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4444",
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2019, 5, 1)
            };

            using (var context = new LuckySlotsDbContext(options))
            {
                var addedCard = await context.CreditCards.AddAsync(card);
                await context.SaveChangesAsync();
                
                var creditCardService = new CreditCardService(context);
                await creditCardService.DeleteAsync(addedCard.Entity.Id.ToString());

                Assert.IsTrue(addedCard.Entity.IsDeleted == true);
            }
        }

        [TestMethod]
        public async Task ThrowsException_When_ObjectDoesntExists()
        {
            var options = GetDbContextOptions("ThrowsException_When_ObjectDoesntExists");
            var userId = Guid.NewGuid().ToString();

            var card = new CreditCard()
            {
                Number = "1111 2222 3333 4444",
                CVV = 123,
                UserId = userId,
                Expiry = new DateTime(2019, 5, 1)
            };

            using (var context = new LuckySlotsDbContext(options))
            {
                var creditCardService = new CreditCardService(context);
                
                var testId = Guid.NewGuid().ToString();

                await Assert.ThrowsExceptionAsync<CreditCardDoesntExistsException>(() =>
                 creditCardService.DeleteAsync(testId));
            }
            
        }
    }
}
