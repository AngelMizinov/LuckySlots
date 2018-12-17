namespace LuckySlots.Infrastructure.Providers
{
    using LuckySlots.Infrastructure.HttpClient;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class JsonParser : IJsonParser
    {
        private readonly IExchangeRateHttpClient client;
        private readonly IMemoryCache cache;

        public JsonParser(IExchangeRateHttpClient client, IMemoryCache cache)
        {
            this.client = client;
            this.cache = cache;
        }

        public async Task<double> ExtractExchangeRate(string rate)
        {
            var result = await GetCacheExchangeRate(rate);

            var exchangeRateJson = JObject.Parse(result);

            return double.Parse(exchangeRateJson["rates"][rate.ToString()].ToString());
        }

        private async Task<string> GetCacheExchangeRate(string rate)
        {
            var result = await this.cache.GetOrCreateAsync("exchangeRate", async entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);

                var exchangeRate = await this.client.GetExchangeRate(rate);

                return exchangeRate;
            });

            return result;
        }
    }
}
