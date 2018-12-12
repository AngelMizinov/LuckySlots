namespace LuckySlots.Services.Tests.GameServicesTests
{
    using LuckySlots.Infrastructure.Contracts;
    using LuckySlots.Infrastructure.Games;
    using LuckySlots.Services.Games;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class Spin_Should
    {
        [TestMethod]
        public void ReturnsException_When_PassedParamaterIsNull()
        {
            // Arrange
            var spinResultMoq = new Mock<ISpinResult>();
            var gamefactoryMoq = new Mock<IGameFactory>();
            var randomizerMoq = new Mock<IRandomizer>();

            Game game = null;

            gamefactoryMoq
                .Setup(g => g.CreateGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(game);

            // Act
            var sut = new GameService(spinResultMoq.Object, gamefactoryMoq.Object, randomizerMoq.Object);

            // Assert
            Assert.ThrowsException<ArgumentException>(() => sut.Spin(game));
        }

        [TestMethod]
        [DataRow("gameofcodes", 4, 3)]
        [DataRow("tuttifrutti", 5, 5)]
        [DataRow("treasuresofegypt", 8, 5)]
        public void Calls_Randomizer(string gameName, int rows, int cols)
        {
            // Arrange
            var spinResultMoq = new Mock<ISpinResult>();
            var gamefactoryMoq = new Mock<IGameFactory>();
            var randomizerMoq = new Mock<IRandomizer>();

            var validGameName = gameName;
            var gameGridHeight = rows;
            var gamegridWidth = cols;

            var game = new Game(gameGridHeight, gamegridWidth);

            gamefactoryMoq
                .Setup(g => g.CreateGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(game);

            randomizerMoq.Setup(mock => mock.Next(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            var sut = new GameService(spinResultMoq.Object, gamefactoryMoq.Object, randomizerMoq.Object);
            var result = sut.Spin(game);

            // Assert
            randomizerMoq.Verify(r => r.Next(It.IsAny<int>(),It.IsAny<int>()));
        }

    }
}
