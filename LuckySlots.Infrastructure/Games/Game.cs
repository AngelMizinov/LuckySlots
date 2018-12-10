namespace LuckySlots.Infrastructure.Games
{
    using System.Collections.Generic;

    public class Game
    {
        public Game(int rows, int cols)
        {
            this.GameGrid = new string[rows, cols];
            this.WinningRows = new List<int>();
        }

        public string[,] GameGrid { get; set; }

        public List<int> WinningRows { get; set; }

        public decimal Winnings { get; set; }
    }
}
