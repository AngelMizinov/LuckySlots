namespace LuckySlots.Services.Admin
{
    using LuckySlots.Data;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using System.Linq;

    public class GameStatsService : BaseService, IGameStatsService
    {
        public GameStatsService(LuckySlotsDbContext context)
            : base(context)
        {
        }

        public int GetNumberOfGamesPlayedByGame(string gameName)
            => this.Context
                .Transactions
                .Where(tr =>
                    tr.Type == "Stake" &&
                    tr.Description.Contains(gameName))
                .Count();

        public decimal GetPaidOutAmountByGame(string gameName)
            => this.Context
                .Transactions
                .Where(tr =>
                    tr.Type == "Win" &&
                    tr.Description.Contains(gameName))
                .Select(tr => tr.BaseCurrencyAmount)
                .Sum();

        public decimal GetStakedAmountByGame(string gameName)
            => this.Context
                .Transactions
                .Where(tr =>
                    tr.Type == "Stake" &&
                    tr.Description.Contains(gameName))
                .Select(tr => tr.BaseCurrencyAmount)
                .Sum();
    }
}
