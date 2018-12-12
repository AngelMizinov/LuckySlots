namespace LuckySlots.Services.Games
{
    using LuckySlots.Infrastructure;
    using LuckySlots.Infrastructure.Contracts;
    using LuckySlots.Infrastructure.Extensions;
    using LuckySlots.Infrastructure.Games;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using System;
    using System.Linq;

    public class GameService : IGameService
    {
        private readonly ISpinResult spinResult;
        private readonly IGameFactory gameFactory;
        private readonly IRandomizer randomizer;

        public GameService(ISpinResult spinResult,
            IGameFactory gameFactory, IRandomizer randomizer)
        {
            this.spinResult = spinResult;
            this.gameFactory = gameFactory;
            this.randomizer = randomizer;
        }

        public Game GetGame(string gameName)
        {
            if (gameName.ToLower() == "gameofcodes")
            {
                return this.gameFactory.CreateGame(4, 3);
            }
            else if (gameName == "tuttifrutti")
            {
                return this.gameFactory.CreateGame(5, 5);
            }
            else if (gameName == "treasuresofegypt")
            {
                return this.gameFactory.CreateGame(8, 5);
            }
            else
            {
                // TODO: Try/catch the exception.
                throw new GameDoesntExistsException("This game does not exists.");
            }
        }

        public float Spin(Game game)
        {
            if (game == null)
            {
                // TODO: Create a custom exception and try/catch it.
                throw new ArgumentException("Game cannot be null!");
            }

            game.WinningRows.Clear();
            var cumulativeCoefficient = 0f;
                
            for (int i = 0; i < game.GameGrid.GetLength(0); i++)
            {
                var currentCummulativeCoefficient = 0f;

                for (int j = 0; j < game.GameGrid.GetLength(1); j++)
                {
                    var outcome = this.randomizer.Next(1, 101) / 100f;
                    var currentFruit = string.Empty;

                    if (outcome <= 0.05)
                    {
                        currentFruit = GlobalConstants.Wildcard;
                        currentCummulativeCoefficient += GlobalConstants.WildcardCoefficient;
                    }
                    else if (outcome <= 0.20)
                    {
                        currentFruit = GlobalConstants.Pineapple;
                        currentCummulativeCoefficient += GlobalConstants.PineappleCoefficient;
                    }
                    else if (outcome <= 0.55)
                    {
                        currentFruit = GlobalConstants.Banana;
                        currentCummulativeCoefficient += GlobalConstants.BananaCoefficient;
                    }
                    else
                    {
                        currentFruit = GlobalConstants.Apple;
                        currentCummulativeCoefficient += GlobalConstants.AppleCoefficient;
                    }

                    game.GameGrid[i, j] = currentFruit;
                }
                
                var distinctOutcomes = game.GameGrid.GetRow(i).Distinct();
                var distinctOutcomesCount = distinctOutcomes.Count();

                if (distinctOutcomesCount > 2 ||
                    (distinctOutcomesCount == 2 && distinctOutcomes.Contains(GlobalConstants.Wildcard) == false))
                {
                    continue;
                }

                game.WinningRows.Add(i);
                cumulativeCoefficient += currentCummulativeCoefficient;
            }

            return cumulativeCoefficient;
        }

        public ISpinResult GetSpinResult(Game game, float coefficient, decimal stake)
        {
            this.spinResult.GameGrid = game.GameGrid;
            this.spinResult.WinningRows = game.WinningRows;
            this.spinResult.Winnings = (decimal)coefficient * stake;

            return this.spinResult;
        }
    }
}
