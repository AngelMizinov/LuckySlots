namespace LuckySlots.Infrastructure.Contracts
{
    using System.Collections.Generic;

    public interface ISpinResult
    {
        string[,] GameGrid { get; set; }

        IList<int> WinningRows { get; set; }

        decimal Winnings { get; set; }
    }
}
