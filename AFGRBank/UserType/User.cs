using AFGRBank.BankAccounts;
using AFGRBank.Exchange;
using AFGRBank.Loans;
using AFGRBank.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.UserType
{
    public class User
    {
        // UserName and Password is used to login
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public bool IsAdmin { get; private set; } = false;
        private decimal TotalFunds { get; set; } = 0;
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> TransactionList { get; set; } = new List<Transaction>();
        public List<Loan> LoanList { get; set; } = new List<Loan>();

        public User()
        {
        }

        // Set the currency of an account (can't be done yet, need to have the conversion rates)
        public void SetCurrency(Account account, CurrencyExchange.CurrencyNames currency)
        {
            account.Currency = currency;
        }

        // Returns a list of all the users accounts
        public List<Account> ViewAccounts()
        {
            return Accounts;
        }

        /// <summary>
        /// Calculates the interest amount for the specified savings account using the provided interest rate.
        /// </summary>
        /// <param name="account">The savings account for which to calculate interest. Must not be null.</param>
        /// <param name="interestRate">The interest rate to apply, expressed as a non-negative decimal fraction (for example, 0.05 for 5%).</param>
        /// <returns>The calculated interest amount for the account, based on its current funds and the specified interest rate.</returns>
        public decimal CalculateAccountInterest(SavingsAccount account, decimal interestRate)
        {
            try
            {
                decimal funds = account.Funds;
                // CurrencyExchange.CurrencyNames currency = account.Currency;
                interestRate = (funds * interestRate);
            }
            catch
            {
                Console.WriteLine("CalculateAccountInterest failed to process information");
            }
            
            return interestRate;
        }

        /* Redundant
        public decimal CalculateLoanInterestRate(Loan loan)
        {
            return loan.InterestRate;
        }
        */

        /// <summary>
        /// Adds a loan to the bank's collection of active loans.
        /// </summary>
        /// <param name="loan">The loan to be added. Must not be null and should represent a valid, eligible loan for the account.</param>
        /// <remarks>
        /// Eligibility for loan is handled by <see cref="Admin.CreateLoan"/>
        /// </remarks>
        public void AddLoan(Loan loan)
        {
            LoanList.Add(loan);
        }

        //This method returns a list of transactions that have occurred in the user's bank accounts.
        public List<Transaction> ViewAllTransactions()
        {
            return TransactionList;
        }

        /// <summary>
        /// Calculates the total sum of funds from all bank accounts
        /// </summary>
        /// <returns>
        /// The total funds as a decimal
        /// </returns>
        /// <remarks>
        /// Loops through every account in the User's account list.
        /// For each account found, adds that amount to TotalFunds (Stored in User)
        /// </remarks>
        public decimal GetTotalFunds()
        {
            try
            {
                foreach (Account account in Accounts)
                {
                    TotalFunds += account.Funds;
                }
            }
            catch
            {
                Console.WriteLine("GetTotalFunds failed to calculate funds");
            }
            
            return TotalFunds;
        }

    }
}
