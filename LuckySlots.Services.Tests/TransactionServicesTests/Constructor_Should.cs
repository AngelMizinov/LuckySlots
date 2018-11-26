namespace LuckySlots.Services.Tests.TransactionServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Transactions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ReturnAnInstance_WhenInvoked()
        {
            // Arrange
            var userStoreMoq = new Mock<IUserStore<User>>().Object;
            var userManagerMoq = new UserManager<User>(userStoreMoq, null, null, null, null, null, null, null, null);
            var dbContextName = Guid.NewGuid().ToString();
            var dbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName));

            // Act
            var instance = new TransactionServices(dbContext, userManagerMoq);

            // Assert
            Assert.IsInstanceOfType(instance, typeof(TransactionServices));
        }

        [TestMethod]
        public void ThrowAnException_WhenUserManagerIsNull()
        {
            // Arrange
            var dbContextName = Guid.NewGuid().ToString();
            var dbContext = new LuckySlotsDbContext(GetDbContextOptions(dbContextName));

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new TransactionServices(dbContext, null));
        }

        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbContextName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
                .UseInMemoryDatabase(dbContextName)
                .Options;
    }
}
