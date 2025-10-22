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
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public List<Account> AccountList { get; set; }
        public List<Transaction> TransactionList { get; set; }
        public List<Loan> LoanList { get; set; }

        public User()
        {
        }

        public void ChooseCurrency()
        {

        }
        public void ViewAccounts()
        {
        }

        public void TransferFunds()
        {
        }

        public void AccountInterestRates()
        {
        }

        public void GetLoanEstimate()
        {
        }

        //This method shows all transactions related to the user across all their accounts.
        public void ViewAllTransactions()
        {
        }


    }
}
