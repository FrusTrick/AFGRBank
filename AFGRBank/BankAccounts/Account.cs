using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;
using AFGRBank.UserType;

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
        public List<User> TransferFunds(List<User> userList, Guid senderAccID, Guid recipientAccID, decimal funds)
        {
            //Fetches both accounts from user list.
            var currentUser = userList.FirstOrDefault(x => x.Accounts.Any(a => a.AccountID == senderAccID));
            Account sender = currentUser.Accounts.FirstOrDefault(x => x.AccountID == senderAccID);

            try
            {
                var recipientUser = userList.FirstOrDefault(x => x.Accounts.Any(a => a.AccountID == recipientAccID));
                Account reciever = currentUser.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID);

                Transaction currentTransaction = new Transaction
                {
                    SenderID = senderAccID,
                    RecieverID = recipientAccID,
                    Funds = funds,
                    TransDate = DateTime.Now
                };

                if (sender != null && reciever != null && sender.Funds > funds)
                {
                    currentUser.Accounts.Where(x => x.AccountID == senderAccID);
                    {
                        //ADD CURRENCY CHECK AND CONVERSION HERE
                        Funds = Funds - funds;
                        currentUser.TransactionList
                    }
                    recipientUser.Accounts.Where(x => x.AccountID == recipientAccID);
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
            catch
            {
                Console.WriteLine($"Could not find a recipient acount with account number: {recipientAccID}");
            }

            //Checks that both accounts have been found and that sender has balance to cover trasnfer.
            //Transfers and confirms if possible. 
            
            
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
