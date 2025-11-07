using AFGRBank.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Loans
{
    public class Loan
    {
        // Represent the currency of the loan.
        public CurrencyExchange.CurrencyNames Currency { get; set; }
        // The interest rate of the loan.
        public decimal InterestRate  { get; set; }
        // The date when the loan starts.
        public DateTime StartDate { get; set; }
        // The date when the loan ends.
        public DateTime EndDate { get; set; }
        // The total amount of the borrowed loan.
        public decimal LoanAmount { get; set; }


        // Create a loan by setting values for the object.
        public void CreateLoan(CurrencyExchange.CurrencyNames currency, decimal interestRate, decimal amount, int months)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(months);
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Displays loan information in the console.
        public void GetLoan()
        {
            Console.WriteLine($"Currency: {Currency}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"End Date: {EndDate}");
            Console.WriteLine($"Loan Amount: {LoanAmount}"); 
        }

        // Update the loan with setting new values.
        public void EditLoan(CurrencyExchange.CurrencyNames currency, decimal interestRate, decimal amount, int months)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(months);
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Calculates the maximum possible loan based on income and multiplier.
        public void CalcMaxLoan(decimal income, decimal multiplier)
        {
            LoanAmount = (income * multiplier);
            Console.WriteLine($"Maximum loan calculated: {LoanAmount}");
        }

        //  // Change this... 
        //  public string Currency { get; set; } 
        //
        //  // To this...
        //  public CurrencyExchange.CurrencyNames Currency { get; set; } 
        //
        //  public List<Loan> CreateLoan(CurrencyExchange.CurrencyNames currency, decimal interestRate, DateOnly startDate, DateOnly endDate, decimal loanAmount, List<Loan> loanList)
        //  {
        //      try
        //      {
        //          Loan newLoan = new Loan
        //          {
        //              Curreny = currency;
        //              InterestRate = interestRate;
        //              StartDate = startDate;
        //              EndDate = endDate;
        //              LoanAmount = amount
        //          }
        //
        //          userList.Add(newUser);
        //          Console.WriteLine($"Loan successfully created.");
        //      }
        //      catch (Exception ex)
        //      {
        //          Console.WriteLine($"Loan failed to process: {ex.Message}");
        //      }
        //      return loanList;
        //  }
    }
}
