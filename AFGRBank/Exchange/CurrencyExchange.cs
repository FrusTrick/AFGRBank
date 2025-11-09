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

        // Sends in an enum currency specified and returns the value of it in the CurrencyRates.json file
        public decimal GetExchangeRate(CurrencyNames currency)
        {
            // Ensures the file that is being edited is the one in the currenct project directory
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
            string jsonString = File.ReadAllText(jsonPath);

            // Deserializes the json file and turns it into a dictionary
            var currencyRates = JsonSerializer.Deserialize<Dictionary<CurrencyNames, decimal>>(jsonString);

            return currencyRates[currency];
        }


        public decimal CalculateExchangeRate(string senderCurrencyName, string recipientCurrencyName, decimal amount)
        {
            if (senderCurrencyName == recipientCurrencyName)
            {
                return amount;
            }

            else
            {
                // Locates the CurrencyRates.JSON file
                // Deserializes it and turns it into an accessible dictionary
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
                string jsonString = File.ReadAllText(jsonPath);
                var currencyRates = JsonSerializer.Deserialize<Dictionary<string, decimal>>(jsonString);


                // Gets exchange rate from JSON.
                // Example:
                // if ( senderCurrencyName == "USD" ) then ( senderXCR == 9.39 )
                //
                decimal senderXCR = currencyRates[senderCurrencyName];
                decimal recipientXCR = currencyRates[recipientCurrencyName];


                // Converts amount to SEK first, then converts to recipient currency, using recipient exchange rate.
                // Example:
                // if ( amount == 100USD ) and (recipientCurrencyName == DKK)
                //      ( 100 / 0.11 = 909.00 SEK) 
                //      ( 909 * 0.68 = 618.12 DKK )


                decimal newAmount = amount / senderXCR;
                decimal newNewAmount = newAmount * recipientXCR;

                // Uncomment line below for testing
                // Console.WriteLine($"{amount} : {newAmount} : {newNewAmount}");

                return newNewAmount;
            }
        }



    }
}
