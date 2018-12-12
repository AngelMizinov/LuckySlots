namespace LuckySlots.Services.Tests.GameServicesTests
{
    using LuckySlots.Infrastructure.Contracts;
    using LuckySlots.Infrastructure.Games;
    using LuckySlots.Services.Games;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class GetGame_Should
    {
        [TestMethod]
        public void Return_AnInstance_WhenValidGameNamePassed()
        {
            // Arrange
            var spinResultMoq = new Mock<ISpinResult>();
            var gamefactoryMoq = new Mock<IGameFactory>();
            var randomizerMoq = new Mock<IRandomizer>();

            const int gameGridHeight = 4;
            const int gamegridWidth = 3;
            const string validGameName = "gameofcodes";

            var game = new Game(gameGridHeight, gamegridWidth);

            gamefactoryMoq
                .Setup(g => g.CreateGame(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(game);

            // Act
            var sut = new GameService(spinResultMoq.Object, gamefactoryMoq.Object, randomizerMoq.Object);
            var result = sut.GetGame(validGameName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Game));
        }

        [TestMethod]
        [DataRow("gameofcodes", 4, 3)]
        [DataRow("tuttifrutti", 5, 5)]
        [DataRow("treasuresofegypt", 8, 5)]
        public void Return_CorrectInstance_WhenValidGameNamePassed(
            string gameName, 
            int rows,
            int cols)
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
                .Setup(g => g.CreateGame(gameGridHeight, gamegridWidth))
                .Returns(game);


            // Act
            var sut = new GameService(spinResultMoq.Object, gamefactoryMoq.Object, randomizerMoq.Object);
            var result = sut.GetGame(validGameName);

            // Assert
            Assert.AreEqual(result.GameGrid.GetLength(0), gameGridHeight);
            Assert.AreEqual(result.GameGrid.GetLength(1), gamegridWidth);
        }

        [TestMethod]
        public void Throw_Exception_WhenGameNameNotValid()
        {
            // Arrange
            var spinResultMoq = new Mock<ISpinResult>();
            var gamefactoryMoq = new Mock<IGameFactory>();
            var randomizerMoq = new Mock<IRandomizer>();

            var invalidGameName = "somegame";

            // Act
            var sut = new GameService(spinResultMoq.Object, gamefactoryMoq.Object, randomizerMoq.Object);

            // Assert
            Assert.ThrowsException<GameDoesntExistsException>(() => sut.GetGame(invalidGameName));
        }
    }
}
