using System.Text.Json;
using AFGRBank.Utility;

namespace AFGRBank.Exchange
{
    public class CurrencyExchange
    {
        public enum CurrencyNames
        {
            SEK,
            DKK,
            EUR,
            USD,
            YEN
        }

        public CurrencyExchange()
        {   
        }

        public decimal GetExchangeRate(CurrencyNames currency)
        {
            // Ensures the file that is being edited is the one in the currenct project directory
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
            string jsonString = File.ReadAllText(jsonPath);

            // Deserializes the json file and turns it into a dictionary
            var currencyRates = JsonSerializer.Deserialize<Dictionary<CurrencyNames, decimal>>(jsonString);

            return currencyRates[currency];
        }
    }
}
