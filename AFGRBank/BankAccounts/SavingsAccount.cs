using AFGRBank.Exchange;
using AFGRBank.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.BankAccounts
{
    public class SavingsAccount : Account
    {
        public bool IsSavings { get; set; } = true;
        public decimal InterestRate { get; set; }

        public SavingsAccount()
        {
            
        }

        public override List<Account> CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)
        {
            try
            {
                SavingsAccount newSavingsAcc = new SavingsAccount
                {
                    AccountID = Guid.NewGuid(),
                    Currency = currency,
                    Funds = 0,
                    IsSavings = true,
                    InterestRate = 2,
                    AccTransList = new List<Transaction>()
                };
                accountList.Add(newSavingsAcc);
                Console.WriteLine($"Savings account created with account number {newSavingsAcc.AccountID}");
                return accountList;
            }
            catch
            {
                Console.WriteLine("Failed to create Savings Account");
                return accountList;
            }
        }

        /// <summary>
        /// Recieves a SavingsAccount and a number representing years. Returns projected account value based on the int years entered. 
        /// </summary>
        /// <param name="savingsAcc"></param>
        /// <param name="years"></param>
        public void SavingsForecast(SavingsAccount savingsAcc, int years)
        {
            try
            {
                var interestRate = Convert.ToDouble(savingsAcc.InterestRate);
                var forecast = Convert.ToDouble(savingsAcc.Funds) * Math.Pow((1 + (interestRate / (100 * 1))), 1 * years);

                Console.WriteLine("________________________________________");
                Console.WriteLine("ACCOUNT FORECAST:");
                Console.WriteLine($"Current balance: {savingsAcc.Funds}{savingsAcc.Currency}");
                Console.WriteLine($"Interest rate:   {savingsAcc.InterestRate}%");
                Console.WriteLine($"Balance forecast over {years} years: {forecast}{savingsAcc.Currency}");
                Console.WriteLine("________________________________________");
            }
            catch
            {
                Console.WriteLine("SavingsForecast failed to process information");
            }
        }
    }
}
