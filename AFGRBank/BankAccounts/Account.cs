using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AFGRBank.BankAccounts
{
    public class Account
    {
        public Guid AccountID { get; set; } = Guid.NewGuid();
        public string Currency { get; set; }
        public decimal Funds { get; set; }
        public List<Transaction> MyProperty { get; set; }

        public Account()
        {
            
        }

        public void ViewAccountInfo()
        {

        }
        public void GenerateAccountID()
        {
        }
        public void CreateAccount()
        {

        }
        public void DeleteAccount()
        {
        }
        public void TransferFunds()
        {
        }

        //MEthod shows all transactions related to the specific account.
        public void ViewTransactions()
        {
        }

    }
}
