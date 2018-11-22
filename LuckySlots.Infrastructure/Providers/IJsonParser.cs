namespace LuckySlots.Infrastructure.Providers
{
    using System.Text;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJsonParser
    {
        Task<double> ExtractExchangeRate(string rate);
    }
}
