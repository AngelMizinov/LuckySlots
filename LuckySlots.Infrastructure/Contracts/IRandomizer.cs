namespace LuckySlots.Infrastructure.Contracts
{
    public interface IRandomizer
    {
        int Next(int minValue, int maxValue);
    }
}
