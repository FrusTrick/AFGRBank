using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Exchange;
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
            try
            {
                //Find both sender account and recipient account.
                var sender = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderAccID));
                var recipient = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderAccID));

                //Create new transaction instance to save to accounts later.
                Transaction currentTransaction = new Transaction
                {
                    SenderID = senderAccID,
                    RecieverID = recipientAccID,
                    Funds = funds,
                    TransDate = DateTime.Now
                };

                try
                {
                    //If account retriaval isn't null and the funds sent do not exceed account balance
                    //the funds will be deducted from the sender, converted and aded to the reciever. 
                    //transaction will be saved to each accounts transaction history.
                    if (sender != null && recipient != null && sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).Funds > funds)
                    {
                        //Fetch currencies and convert. Converted currency saved in variable converted rate and
                        //added instead of funds to the recipient account.
                        CurrencyExchange exhange = new CurrencyExchange();
                        string senderCurrency = sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).Currency;
                        string recipientCurrency = recipient.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID).Currency;
                        decimal convertedRate = exhange.CalculateExchangeRate(senderCurrency, recipientCurrency, funds);

                        sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).Funds -= funds;
                        sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).AccTransList.Add(currentTransaction);

                        //currentTransaction updated with recipient currency value to reflect the recieved funds correctly in accordance with the recipient currency.
                        currentTransaction.Funds = convertedRate;
                        recipient.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID).Funds += convertedRate;
                        recipient.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID).AccTransList.Add(currentTransaction);

                        //Replaces values of acounts in the userlist with updated values, using index to identify
                        //their locations and overwriting the old data with the updated sender and recipient data.
                        var indexSender = userList.IndexOf(sender);
                        var indexRecipient = userList.IndexOf(recipient);
                        if (indexSender > -1 && indexRecipient > -1)
                        {
                            userList[indexSender] = sender;
                            userList[indexRecipient] = recipient;
                        }

                        return userList;
                    }
                }
                catch
                {
                    Console.WriteLine("Transaction failed");
                    return userList;
                }

            }
            catch
            {
                Console.WriteLine($"Could not find the account with account number:{recipientAccID}");
                return userList;
            }

            return userList;
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
