using AFGRBank.BankAccounts;
using AFGRBank.Exchange;
using AFGRBank.UserType;
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
        /// <summary>
        /// Creates a new loan with specified currency, interest rate amount, and duration in months</summary>
        /// <param name="currency">The currency of the loan</param>
        /// <param name="interestRate">The interest rate of the loan.</param>
        /// <param name="amount">The principal amount of the loan.</param>
        /// <param name="months">The duration of the loan in months.</param>
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
        /// <summary>
        /// Displays information about the current loan in the conosle.
        /// </summary>
        public void GetLoan()
        {
            Console.WriteLine($"Currency: {Currency}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"End Date: {EndDate}");
            Console.WriteLine($"Loan Amount: {LoanAmount}"); 
        }


        // Update the loan with setting new values.
        /// <summary>
        /// Updates the loan with new values for currency, interest rate, amout, and duration.
        /// </summary>
        /// <param name="currency">The new currency of the loan.</param>
        /// <param name="interestRate">The new interest rate of the loan.</param>
        /// <param name="amount">The new principal amount of the loan.</param>
        /// <param name="months">The new duration of the loan in months.</param>
        public void EditLoan(CurrencyExchange.CurrencyNames currency, decimal interestRate, decimal amount, int months)
        {
            Currency = currency;
            InterestRate = interestRate;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(months);
            LoanAmount = amount;
            Console.WriteLine("Loan created successfully. ");
        }

        // Determine the max loan amount you can get based on your current income.
        /// <summary>
        /// Calculates the maximum possible loan based on income and a multiplier.
        /// </summary>  
        /// <param name="income">The income used to calculate the loan.</param>
        /// <param name="multiplier">The multiplier applied to the income to determine the loan amount.</param>
        public void CalcMaxLoan(decimal income, decimal multiplier)
        {
            LoanAmount = (income * multiplier);
            Console.WriteLine($"Maximum loan calculated: {LoanAmount}");
        }

        /// <summary>
        ///     Displays all loans in user
        /// </summary>
        /// <remarks>
        ///     If no loans exists in user's loan list are found, a message informing of that will be displayed instead
        /// </remarks>
        /// <param name="user"></param>
        public void DisplayAllLoans(User user)
        {
            List<Loan> allLoans = new List<Loan>();

            foreach (Loan loan in user.LoanList)
            {
                allLoans.Add(loan);
            }

            if (allLoans.Count > 0)
            {

                foreach (var loan in allLoans)
                {
                    Console.WriteLine($"________________________________________________________");
                    Console.WriteLine($"You borrowed:  {loan.LoanAmount} {loan.Currency}");
                    Console.WriteLine($"Interest rate: {loan.InterestRate}");
                    Console.WriteLine($"Date of loan:  {loan.StartDate} - {loan.EndDate}");
                    Console.WriteLine($"________________________________________________________");

                }
            }
            else
            {
                Console.WriteLine("Your loan history is empty.");
            }
        }
    }
}
