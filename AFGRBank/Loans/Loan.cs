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
        public string Currency { get; set; }
        public decimal InterestRate  { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int LoanAmount { get; set; }

        
        // Konstruktor som skapar ett nytt lån med alla värden direkt
        public Loan(string currency, decimal interestRate, DateOnly startDate, DateOnly endDate, int amount)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = startDate;
            EndDate = endDate;
            LoanAmount = amount;
        }

        // Skapar ett lån genom att sätta värden för objektet
        public void CreateLoan(string currency, decimal interestRate, DateOnly startDate, DateOnly endDate, int amount)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = startDate;
            EndDate = endDate;
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Visar låneinformation i konsolen
        public void GetLoan()
        {
            Console.WriteLine($"Currency: {Currency}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"End Date: {StartDate}");
            Console.WriteLine($"Loan Amount: {LoanAmount}");
        }

        // Redigerar ett lån genom att uppdatera objektets värden
        public void EditLoan(string currency, decimal interestRate, DateOnly startDate, DateOnly endDate, int amount)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = startDate;
            EndDate = endDate;
            LoanAmount = amount;
            Console.WriteLine("Loan edited successfully.");
        }

        // Beräknar maxlån baserat på inkomst och en multiplikator
        public void CalcMaxLoan(decimal income, decimal multiplier)
        {
            LoanAmount = (int)(income * multiplier); // Maxlån = inkomst * multiplikator
            Console.WriteLine($"Maximum loan calculated: {LoanAmount}");
        }

    }
}
