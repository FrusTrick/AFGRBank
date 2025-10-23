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
        public List<SavingsAccount> savingsAccounts { get; set; }
        public List<CheckingsAccount> checkingAccounts { get; set; }
        public List<Transaction> TransactionList { get; set; }
        public List<Loan> LoanList { get; set; }

        public User()
        {
        }

        // Set the currency of an account (can't be done yet, need to have the conversion rates)
        public void SetCurrency(Account account, string currency)
        {
            account.Currency = currency;
        }

        // Returns a list of all the users accounts
        public List<Account> ViewAccounts(List<Account> accountList)
        {
            return accountList;
        }

        // Calculates the interest rate for the specified account
        public decimal AccountInterestRates(SavingsAccount account, decimal interestRate)
        {
            decimal funds = account.Funds;
            string currency = account.Currency;
            decimal expectedInterest = (funds * interestRate);
            return expectedInterest;
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


    }
}
