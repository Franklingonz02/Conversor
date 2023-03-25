namespace conversor.Service
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class CurrencyConverterService : Controller
    {
        private readonly HttpClient _httpClient;

        public CurrencyConverterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/latest?base={fromCurrency}&symbols={toCurrency}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to convert currency: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesResponse>(content);

            if (!exchangeRates.Rates.ContainsKey(toCurrency))
            {
                throw new Exception($"Invalid currency code: {toCurrency}");
            }

            var exchangeRate = exchangeRates.Rates[toCurrency];
            var convertedAmount = amount * exchangeRate;

            return Math.Round(convertedAmount, 2);
        }

        private class ExchangeRatesResponse
        {
            public string Base { get; set; }
            public DateTime Date { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }

}
