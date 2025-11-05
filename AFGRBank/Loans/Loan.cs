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
        // Egenskaer för lånet
        public CurrencyExchange.CurrencyNames Currency { get; set; }
        public decimal InterestRate  { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal LoanAmount { get; set; }

        
       

        // Skapar ett lån genom att sätta värden för objektet
        public void CreateLoan(CurrencyExchange.CurrencyNames currency, decimal interestRate, decimal amount, int months)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(months);
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Visar låneinformation i konsolen
        public void GetLoan()
        {
            Console.WriteLine($"Currency: {Currency}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"End Date: {EndDate}");
            Console.WriteLine($"Loan Amount: {LoanAmount}");
            
        }

        // Redigerar ett lån genom att uppdatera objektets värden
        public void EditLoan(CurrencyExchange.CurrencyNames currency, decimal interestRate, decimal amount, int months)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(months);
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Beräknar maxlån baserat på inkomst och en multiplikator
        public void CalcMaxLoan(decimal income, decimal multiplier)
        {
            LoanAmount = (income * multiplier); // Maxlån = inkomst * multiplikator
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
