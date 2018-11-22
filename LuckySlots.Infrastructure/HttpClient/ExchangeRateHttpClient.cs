namespace LuckySlots.Infrastructure.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Http;
    
    public class ExchangeRateHttpClient : IExchangeRateHttpClient
    {
        private const string BaseCurrency = "USD";
        private const string ApiUrl = "https://api.exchangeratesapi.io/latest?base={0}&symbols={1}";
                                     
        private readonly HttpClient client;
        
        public ExchangeRateHttpClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetExchangeRate(string symbol)
        {
            var apiCallUrl = string.Format(ApiUrl, BaseCurrency, symbol);
            
            return await this.client.GetStringAsync(apiCallUrl);
        }
    }
}
