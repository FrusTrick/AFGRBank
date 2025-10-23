using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AFGRBank.BankAccounts;
using AFGRBank.Loans;

namespace AFGRBank.UserType
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        private decimal TotalFunds { get; set; } = 0;
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> TransactionList { get; set; } = new List<Transaction>();
        public List<Loan> LoanList { get; set; } = new List<Loan>();

        public User()
        {
        }

        // Set the currency of an account (can't be done yet, need to have the conversion rates)
        public void SetCurrency(Account account, string currency)
        {
            account.Currency = currency;
        }

        // Returns a list of all the users accounts
        public List<Account> ViewAccounts()
        {
            return Accounts;
        }

        // Calculates the interest rate for the specified account
        public decimal CalculateAccountInterest(SavingsAccount account, decimal interestRate)
        {
            try
            {
                decimal funds = account.Funds;
                string currency = account.Currency;
                interestRate = (funds * interestRate);
            }
            catch
            {
                Console.WriteLine("CalculateAccountInterest failed to process information");
            }
            
            return interestRate;
        }

        // Returns interest rate
        public decimal CalculateLoanInterestRate(Loan loan)
        {
            return loan.InterestRate;
        }

        //Method loans out x amount of funds from the bank, to x account, at an x rate
        //Also checks if the user is eligible for a loan
        public void AddLoan(Loan loan)
        {
            LoanList.Add(loan);
        }

        //This method returns a list of transactions that have occurred in the user's bank accounts.
        public List<Transaction> ViewAllTransactions()
        {
            return TransactionList;
        }

        // Calculates the total funds across all bank account types
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
