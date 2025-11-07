using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Exchange;
using AFGRBank.Main;

namespace AFGRBank.BankAccounts
{
    public class CheckingsAccount : Account
    {
        public bool isCheckings { get; set; } = true;

        public override List<Account> CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)
        {
            try
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

                Console.WriteLine($"Checkings account created with with account number {newCheckingsAcc.AccountID}");
                return accountList;
            }
            catch
            {
                Console.WriteLine("Failed to create Checkings Account");
                return accountList;
            }
        }

    }

}
