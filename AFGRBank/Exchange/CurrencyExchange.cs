using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AFGRBank.Exchange
{
    public class CurrencyExchange
    {
        public enum CurrencyName { SEK, DKK, EUR, USD, YEN }

        public CurrencyExchange()
        {   
        }

        public decimal GetExchangeRate(CurrencyName currency)
        {
            // Ensures the file that is being edited is the one in the currenct project directory
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
            string jsonString = File.ReadAllText(jsonPath);

            // Deserializes the json file and turns it into a dictionary
            var currencyRates = JsonSerializer.Deserialize<Dictionary<CurrencyName, decimal>>(jsonString);

            return currencyRates[currency];
        }
    }
}
