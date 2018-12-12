namespace LuckySlots.Services.Tests.GameServicesTests
{
    using LuckySlots.Infrastructure.Contracts;
    using LuckySlots.Infrastructure.Games;
    using LuckySlots.Services.Games;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [TestClass]
    public class GetSpinResult_Should
    {
        // TODO: Test about GetSpinResult method
        [TestMethod]
        [DataRow(4, 3)]
        [DataRow(5, 5)]
        [DataRow(8, 5)]
        public void Returns_InstanceOfSpinResult(int rows, int cols)
        {
            // Arrange
            var spinResult = new SpinResult();
            var gamefactoryMoq = new Mock<IGameFactory>();
            var randomizerMoq = new Mock<IRandomizer>();

            var gameGridHeight = rows;
            var gamegridWidth = cols;

            float coefficient = 0.7f;
            decimal stake = 10;

            var game = new Game(gameGridHeight, gamegridWidth);
            
            // Act
            var sut = new GameService(spinResult, gamefactoryMoq.Object, randomizerMoq.Object);
            var result = sut.GetSpinResult(game, coefficient, stake);

            // Assert
            Assert.IsInstanceOfType(result, typeof(SpinResult));
        }

    }
}
