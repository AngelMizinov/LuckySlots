namespace LuckySlots.Infrastructure.Games
{
    using System;
    using System.Linq;

    public class SlotMachine
    {
        private string [,] gameGrid;

        public SlotMachine(int rows, int cols)
        {
            this.gameGrid = new string[rows, cols];
        }

        public void Spin()
        {
            for (int i = 0; i < gameGrid.GetLength(0); i++)
            {
                for (int j = 0; j < gameGrid.GetLength(1); j++)
                {
                    var outcome = GenerateOutcome() / 100;
                    var cumulativeProbability = GlobalConstatnts.WildcardProbability;

                    var fruit = "";

                    if (outcome <= cumulativeProbability)
                    {
                        fruit = GlobalConstatnts.Wildcard;
                        cumulativeProbability += GlobalConstatnts.PineappleProbability;
                    }
                    else if (outcome <= cumulativeProbability)
                    {
                        fruit = GlobalConstatnts.Pineapple;
                        cumulativeProbability += GlobalConstatnts.BananaProbability;
                    }
                    else if (outcome <= cumulativeProbability)
                    {
                        fruit = GlobalConstatnts.Banana;
                        cumulativeProbability += GlobalConstatnts.AppleProbability;
                    }
                    else
                    {
                        fruit = GlobalConstatnts.Apple;
                    }

                    gameGrid[i, j] = fruit;
                }
            }
        }

        private int GenerateOutcome()
        {
            var randomizer = new Random(Guid.NewGuid().GetHashCode());

            return randomizer.Next(1, 101);
        }
    }
}
