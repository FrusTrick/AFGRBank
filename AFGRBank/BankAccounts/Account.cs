using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;

namespace AFGRBank.BankAccounts
{
    public class Account
    {
        public Guid AccountID { get; set; } 
        public string Currency { get; set; }
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



        public virtual List<Account> CreateAccount(List<Account> accountList, string currency)
        {
            return accountList;
        }

        //Removes an account from the user account list. Checks if account 
        public List<CheckingsAccount> DeleteAccount(List<CheckingsAccount> accountList, Guid accountId)
        {
            foreach (CheckingsAccount account in accountList) 
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
            return accountList;
        }

        //Transfers funds between two accounts.
        //ADD CURRENCY CHECK AND CONVERSION!!!!!!
        public List<Account> TransferFunds(List<Account> accountList, Guid senderAccID, Guid recipientAccID, decimal funds)
        {
            //Fetches both accounts from account list.
            Account sender = accountList.FirstOrDefault(x => x.AccountID == senderAccID);
            Account reciever = new Account();

            try
            {
                reciever = accountList.FirstOrDefault(x => x.AccountID == recipientAccID);
            }
            catch{Console.WriteLine($"Could not find a recipient acount with account number: {recipientAccID}");}

            //Checks that both accounts have been found and that sender has balance to cover trasnfer.
            //Transfers and confirms if possible. 
            if (sender != null && reciever != null && sender.Funds > funds) 
            {
                accountList.Where(x => x.AccountID == senderAccID);
                {
                    //ADD CURRENCY CHECK AND CONVERSION HERE
                    Funds = Funds - funds;
                }
                accountList.Where(x => x.AccountID == recipientAccID);
                {
                    //ADD CURRENCY CHECK AND CONVERSION HERE
                    Funds = Funds + funds;
                }
                Console.WriteLine($"You have transfered {funds} to {recipientAccID}");

                //Returns updated values.
                return accountList;
            }
            else
            {
                //Returns unchanged values.
                return accountList;
            }
            
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
                    Console.WriteLine($"Recipient account: {transaction.RecieverID}");
                    Console.WriteLine("________________________________________");
                }
            }
            
        }

    }
}
