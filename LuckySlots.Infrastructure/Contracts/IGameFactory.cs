namespace LuckySlots.Infrastructure.Contracts
{
    using LuckySlots.Infrastructure.Games;

    public interface IGameFactory
    {
        Game CreateGame(int rows, int cols);
    }
}
