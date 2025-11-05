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
        // properties of the loan
        public string Currency { get; set; }
        public decimal InterestRate  { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal LoanAmount { get; set; }

        
       

        // Creating a loan by setting values  for the object
        public void CreateLoan(string currency, decimal interestRate, DateOnly startDate, DateOnly endDate, int amount)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = startDate;
            EndDate = endDate;
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Showing loan information in the console
        public void GetLoan()
        {
            Console.WriteLine($"Currency: {Currency}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"End Date: {EndDate}");
            Console.WriteLine($"Loan Amount: {LoanAmount}");
            
        }

        // Edits a loan by updating the objects values
        public void EditLoan(string currency, decimal interestRate, DateOnly startDate, DateOnly endDate, int amount)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = startDate;
            EndDate = endDate;
            LoanAmount = amount;
            Console.WriteLine("Loan edited successfully.");
        }

        // Calvulate maximum loan based on income and multiplier
        public void CalcMaxLoan(decimal income, decimal multiplier)
        {
            LoanAmount = (income * multiplier); // Maxlån = inkomst * multiplikator
            Console.WriteLine($"Maximum loan calculated: {LoanAmount}");
        }

    }
}
