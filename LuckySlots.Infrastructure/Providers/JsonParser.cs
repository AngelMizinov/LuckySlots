namespace LuckySlots.Infrastructure.Providers
{
    using LuckySlots.Infrastructure.HttpClient;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class JsonParser : IJsonParser
    {
        private readonly IExchangeRateHttpClient client;

        public JsonParser(IExchangeRateHttpClient client)
        {
            this.client = client;
        }

        public async Task<double> ExtractExchangeRate(string rate)
        {
            var result = await this.client.GetExchangeRate(rate);

            var exchangeRateJson = JObject.Parse(result);

            return double.Parse(exchangeRateJson["rates"][rate.ToString()].ToString());
        }
    }
}
