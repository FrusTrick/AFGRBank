using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.CurrencyHandler
{
    public class CurrencyExchange
    {
        public enum CurrencyName { SEK, DKK, EUR, USD, YEN }
        public string Currency { get; set; }
        public decimal Rate { get; set; }

        public CurrencyExchange()
        {   
        }

        public decimal GetExchangeRate(CurrencyName currency)
        {
            return 1;
        }
    }
}
