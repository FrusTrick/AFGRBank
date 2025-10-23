using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;
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
            List<Account> allAccounts = new();
            allAccounts.AddRange(Accounts);
            return allAccounts;
        }

        // Calculates the interest rate for the specified account
        public decimal CalculateAccountInterest(SavingsAccount account, decimal interestRate)
        {
            decimal funds = account.Funds;
            string currency = account.Currency;
            decimal expectedInterest = (funds * interestRate);
            return expectedInterest;
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
            decimal totalFunds = 0;
            foreach (Account account in Accounts)
            {
                totalFunds += account.Funds;
            }
            return totalFunds;
        }

    }
}
