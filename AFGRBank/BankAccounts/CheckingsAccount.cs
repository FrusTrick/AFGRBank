using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;

namespace AFGRBank.BankAccounts
{
    public class CheckingsAccount : Account
    {
        public bool isCheckings { get; set; } = true;

        public override List<Account> CreateAccount(List<Account> accountList, string currency)
        {
            CheckingsAccount newCheckingsAcc = new CheckingsAccount
            {
                AccountID = Guid.NewGuid(),
                Currency = currency,
                Funds = 0,
                isCheckings = true,
                AccTransList = new List<Transaction>()
            };
            accountList.Add(newCheckingsAcc);

            Console.WriteLine($"Checkings account with account number {newCheckingsAcc.AccountID} has been created");
            return accountList;
        }

    }

}
