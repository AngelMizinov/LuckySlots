namespace LuckySlots.Infrastructure.Games
{
    using LuckySlots.Infrastructure.Contracts;

    public class GameFactory : IGameFactory
    {
        public Game CreateGame(int rows, int cols)
        {
            return new Game(rows, cols);
        }
    }
}
