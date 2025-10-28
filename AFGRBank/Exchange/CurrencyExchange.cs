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


        public decimal CalculateExchangeRate(string senderCurrencyName, string recipientCurrencyName, decimal transactionAmount)
        {
            if (senderCurrencyName == recipientCurrencyName)
            {
                return transactionAmount;
            }

            else
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
                string jsonString = File.ReadAllText(jsonPath);

                // Deserializes the json file and turns it into a dictionary
                var currencyRates = JsonSerializer.Deserialize<Dictionary<string, decimal>>(jsonString);


                // Gets exchange rate from JSON.
                // Example:
                // if ( senderCurrencyName == "USD" ) then ( senderXCR == 9.39 )
                //
                decimal senderXCR = currencyRates[senderCurrencyName];
                decimal recipientXCR = currencyRates[recipientCurrencyName];


                // Convert transactionAmount to SEK first (newTransactionAmount)
                // Then converts newTransactionAmount to recipient, using recipient exchange rate.
                // Example:
                // if ( transactionAmount == 100USD ) then ( 100 / 0.11 = 909 SEK)

                // if ( transactionAmount == 100USD ) then ( 100 * 0.11 = 11 SEK)

                decimal newTransactionAmount = transactionAmount / senderXCR;
                decimal newNewTransactionAmount = newTransactionAmount * recipientXCR;
                
                return newNewTransactionAmount;
            }
        }



    }
}
