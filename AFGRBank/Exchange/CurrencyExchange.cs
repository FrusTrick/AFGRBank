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

        public List<string> FetchCurrencyNameList()
        {
            var list = Enum.GetValues<CurrencyName>();
            List<string> list2 = new();
            foreach (var item in list)
            {
                list2.Add(item.ToString());
            }
            return list2;
        }


        public decimal CalculateExchangeRate(string senderCurrencyName, string recipientCurrencyName, decimal transactionAmount)
        {
            if (senderCurrencyName == recipientCurrencyName)
            {
                return transactionAmount;
            }

            else
            {
                string jsonString = File.ReadAllText("./Exchange/CurrencyRates.json");

                Dictionary<string, decimal> listExchangeRate =
                    JsonSerializer.Deserialize<Dictionary<string, decimal>>(jsonString);

                decimal senderExchangeRate = listExchangeRate[senderCurrencyName];
                decimal recipientExchangeRate = listExchangeRate[recipientCurrencyName];

                
                // Convert transactionAmount to SEK first
                decimal newTransactionAmount = transactionAmount / senderExchangeRate;

                // Converts transactionAmount in SEK to exchange rate of the recipient's bank currency.
                decimal newNewTransactionAmount = transactionAmount * recipientExchangeRate;

                switch (senderCurrencyName)
                {

                }
            }
            return ;
        }



    }
}
