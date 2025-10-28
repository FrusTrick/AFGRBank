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
            string jsonString = File.ReadAllText("./Exchange/CurrencyRates.json");

            Dictionary<CurrencyName, decimal> currencyExchange =
                JsonSerializer.Deserialize<Dictionary<CurrencyName, decimal>>(jsonString);

            return currencyExchange[currency];
        }
    }
}
