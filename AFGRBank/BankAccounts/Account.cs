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
        public CurrencyExchange.CurrencyNames Currency { get; set; }
        public decimal Funds { get; set; }
        public List<Transaction> AccTransList { get; set; }

        public Account()
        {
            
        }

        //Method that prints general account info to the console.
        /// <summary>
        /// Displays the account information including AccountID, Currency, and Funds to the console.
        /// Takes an Account object as a parameter.
        /// </summary>
        /// <param name="account"></param>
        public virtual void ViewAccountInfo(Account account)
        {
            try 
            {
                Console.WriteLine("________________________________________");
                Console.WriteLine($"Account number: {account.AccountID} \nCurrency: {account.Currency} \nBalance: {account.Funds}");
                Console.WriteLine("________________________________________");
            }
            catch 
            {                
                Console.WriteLine("ViewAccountInfo failed to process information");
            }
        }



        public virtual List<Account> CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)
        {
            try
            {
                Account newAccount = new Account
                {
                    AccountID = Guid.NewGuid(),
                    Currency = currency,
                    Funds = 0,
                    AccTransList = new List<Transaction>()
                };
                Console.WriteLine($"New account created with account number {newAccount.AccountID}");
                return accountList;
            }
            catch
            {
                Console.WriteLine("CreateAccount failed to process information");
                return accountList;
            }
        }

        //Removes an account from the user account list. Checks if account ID matches the recieved one, then checks the available funds.
        //If available funds in 0, the account will be closed.
        /// <summary>
        /// Removes an account from the specified account list if the account ID matches and the account has no
        /// available funds. Lets the user confirm the deletion before proceeding.
        /// </summary>
        /// <remarks>If the account with the specified <paramref name="accountId"/> is found and its
        /// available funds are zero, the account is removed from the list. If the account still has funds, it cannot be
        /// removed, and a message is displayed prompting the user to transfer the funds. If no account matches the
        /// specified ID, a message is displayed indicating that the account could not be found.</remarks>
        /// <param name="accountList">The list of accounts to search for the account to be removed.</param>
        /// <param name="accountId">The unique identifier of the account to be removed.</param>
        /// <returns>The updated list of accounts after attempting to remove the specified account.</returns>
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
        /// <summary>
        /// Takes a list of Users, account GUIDs for sender and recipient, and the amount of funds to be transfered.
        /// Returns the updated user list with the modified account balances and transaction histories.
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="senderAccID"></param>
        /// <param name="recipientAccID"></param>
        /// <param name="funds"></param>
        /// <returns></returns>
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
                    ReceiverID = recipientAccID,
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
                        CurrencyExchange.CurrencyNames toSenderCurrency = sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).Currency;
                        CurrencyExchange.CurrencyNames toRecipientCurrency = recipient.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID).Currency;

                        // Alex: Converts enums to string
                        string senderCurrency = toSenderCurrency.ToString();
                        string recipientCurrency = toRecipientCurrency.ToString();
                        
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
        /// <summary>
        /// Displays the transaction history for the specified account.
        /// </summary>
        /// <remarks>This method outputs the transaction history to the console, including details such as
        /// the transaction  date, transferred funds, and recipient account for each transaction. If the account's
        /// transaction list  is null or empty, no transactions will be displayed.</remarks>
        /// <param name="account">The account whose transaction history will be displayed. The account must not be null, and its  <see
        /// cref="Account.AccTransList"/> property should contain the list of transactions to display.</param>
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
