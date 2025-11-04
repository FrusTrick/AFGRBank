using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;
{
    
}

namespace AFGRBank.BankAccounts
{
    public class SavingsAccount : Account
    {
        public bool IsSavings { get; set; } = true;
        public decimal InterestRate { get; set; }

        public SavingsAccount()
        {
            
        }

        public override List<Account> CreateAccount(List<Account> accountList, string currency)
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
            Console.WriteLine($"Savings account with account number {newSavingsAcc.AccountID} has been created");
            return accountList;
        }

        /// <summary>
        /// Recieves a SavingsAccount and a number representing years. Returns projected account value based on the int years entered. 
        /// </summary>
        /// <param name="savingsAcc"></param>
        /// <param name="years"></param>
        public void SavingsForecast(SavingsAccount savingsAcc, int years)
        {
            var interestRate= Convert.ToDouble(savingsAcc.InterestRate);
            var forecast = Convert.ToDouble(savingsAcc.Funds) * Math.Pow((1 + (interestRate / (100 * 1))), 1 * years);

            Console.WriteLine("________________________________________");
            Console.WriteLine("ACCOUNT FORECAST:");
            Console.WriteLine($"Current balance: {savingsAcc.Funds}{savingsAcc.Currency}");
            Console.WriteLine($"Interest rate:   {savingsAcc.InterestRate}%");
            Console.WriteLine($"Balance forecast over {years} years: {forecast}{savingsAcc.Currency}");
            Console.WriteLine("________________________________________");
        }
    }
}
