namespace LuckySlots.Services.Contracts
{
    public interface IGameStatsService
    {
        int GetNumberOfGamesPlayedByGame(string gameName);

        decimal GetStakedAmountByGame(string gameName);

        decimal GetPaidOutAmountByGame(string gameName);
    }
}
