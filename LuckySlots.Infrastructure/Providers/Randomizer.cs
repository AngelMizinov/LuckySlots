namespace LuckySlots.Infrastructure.Providers
{
    using LuckySlots.Infrastructure.Contracts;
    using System;

    public class Randomizer : IRandomizer
    {
        public int Next(int minValue, int maxValue)
        {
            var randomizer = new Random(Guid.NewGuid().GetHashCode());
            return randomizer.Next(minValue, maxValue);
        }
    }
}
