using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Exchange;
using AFGRBank.Main;
using AFGRBank.UserType;
using AFGRBank.Utility;

namespace AFGRBank.BankAccounts
{
    public class Account
    {
        public Guid AccountID { get; set; } 
        public CurrencyExchange.CurrencyNames Currency { get; set; }
        public decimal Funds { get; set; }
        public List<Transaction> AccTransList { get; set; }

        public Account()
        {
            
        }

        //Method that prints general account info to the console.
        public virtual void ViewAccountInfo(Account account)
        {
            Console.WriteLine("________________________________________");
            Console.WriteLine($"Account number: {account.AccountID} \nCurrency: {account.Currency} \nBalance: {account.Funds}");
            Console.WriteLine("________________________________________");
        }



        public virtual List<Account> CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)
        {
            return accountList;
        }

        //Removes an account from the user account list. Checks if account ID matches the recieved one, then checks the available funds.
        //If available funds in 0, the account will be closed.
        /// <summary>
        /// Removes an account from the specified account list if the account ID matches and the account has no
        /// available funds.
        /// </summary>
        /// <remarks>The method prompts the user for confirmation before attempting to close the account. 
        /// If the account has funds, it cannot be closed, and the user is notified.  If the account ID does not match
        /// any account in the list, the user is informed.</remarks>
        /// <param name="accountList">The list of accounts to search for the account to be removed.</param>
        /// <param name="accountId">The unique identifier of the account to be removed.</param>
        /// <returns>The updated list of accounts after attempting to remove the specified account.  If the account cannot be
        /// removed, the original list is returned.</returns>
        public List<Account> DeleteAccount(List<Account> accountList, Guid accountId)
        {
            int menu = Menu.ReadOptionIndexList("Are you sure you want to close this account?", new List<string> { "Yes", "No" });

            if(menu == 1)
            {
                Console.WriteLine("Account closure cancelled.");
                return accountList;
            }
            else
            {
                foreach (Account account in accountList)
                {
                    if (account.AccountID == accountId)
                    {
                        if (account.Funds == 0)
                        {
                            accountList.Remove(account);
                            Console.WriteLine("Account has been successfully closed");
                        }
                        else
                        {
                            Console.WriteLine("The account you are trying to close still has funds in it, please transfer the funds from the account and try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot find an account with the given AccountID");
                    }
                }
            }
                
            return accountList;
        }

        

        //Method shows all transactions related to the specific account.
        public void ViewTransactions(Account account)
        {
            Console.WriteLine($"Transaction history for {AccountID}:");
            if (account.AccTransList != null)
            {
                foreach(Transaction transaction in account.AccTransList)
                {
                    Console.WriteLine("________________________________________");
                    Console.WriteLine($"Transaction date: {transaction.TransDate.ToShortTimeString()}");
                    Console.WriteLine($"Transfered funds: {transaction.Funds}{account.Currency}");
                    Console.WriteLine($"Recipient account: {transaction.ReceiverID}");
                    Console.WriteLine("________________________________________");
                }
            }
            
        }

    }
}
