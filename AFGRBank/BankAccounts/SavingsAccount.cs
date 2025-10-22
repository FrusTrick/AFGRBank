using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AFGRBank.BankAccounts
{
    public class SavingsAccount : Account
    {
        public bool isSavings { get; set; } = true;
        public decimal InterestRate { get; set; }

        public SavingsAccount()
        {
            
        }

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
