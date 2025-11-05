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


        /// <summary>
        /// Takes a list of accounts and a currency string as parameters. 
        /// Creates a new CheckingsAccount with default values and adds it to the incoming account list.
        /// Returns the updated account list.
        /// </summary>
        /// <param name="accountList"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public override List<Account> CreateAccount(List<Account> accountList, string currency)
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
