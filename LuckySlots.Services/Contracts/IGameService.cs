namespace LuckySlots.Services.Contracts
{
    using LuckySlots.Infrastructure.Contracts;
    using LuckySlots.Infrastructure.Games;

    public interface IGameService
    {
        Game GetGame(string gameName);

        float Spin(Game game);

        ISpinResult GetSpinResult(Game game, float coefficient, decimal stake);
    }
}
