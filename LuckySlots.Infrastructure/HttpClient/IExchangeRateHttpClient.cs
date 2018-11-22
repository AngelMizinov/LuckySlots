using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySlots.Infrastructure.HttpClient
{
    public interface IExchangeRateHttpClient
    {
        Task<string> GetExchangeRate(string symbol);
        
    }
}
