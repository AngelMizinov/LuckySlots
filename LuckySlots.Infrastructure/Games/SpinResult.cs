namespace LuckySlots.Infrastructure.Games
{
    using LuckySlots.Infrastructure.Contracts;
    using System.Collections.Generic;

    public class SpinResult : ISpinResult
    {
        public string[,] GameGrid { get; set; }

        public IList<int> WinningRows { get; set; }

        public decimal Winnings { get; set; }
    }
}
