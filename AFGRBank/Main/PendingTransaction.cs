using AFGRBank.Exchange;
using AFGRBank.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Main
{
    public class PendingTransaction
    {
        public List<Transaction> CurrentTransaction { get; set; }
        public bool Confirmed { get; set; } = false;

        public PendingTransaction()
        {

        }

        public PendingTransaction(List<Transaction> transaction)
        {
            CurrentTransaction = transaction;
        }

        //Transfers funds between two accounts.
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="senderAccID"></param>
        /// <param name="recipientAccID"></param>
        /// <param name="funds"></param>
        /// <returns></returns>
        public List<Transaction> PrepFundsTransfer(List<User> userList, Guid senderAccID, Guid recipientAccID, decimal funds)
        {
            try
            {
                //Find both sender account and recipient account.
                var sender = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderAccID));
                var recipient = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == recipientAccID));

                //Create new transaction instance to save to accounts later.
                Transaction senderTransaction = new Transaction
                {
                    SenderID = senderAccID,
                    ReceiverID = recipientAccID,
                    Funds = funds,
                    TransDate = DateTime.Now
                };
                Transaction recipientTransaction = new Transaction
                {
                    SenderID = senderAccID,
                    ReceiverID = recipientAccID,
                    Funds = funds,
                    TransDate = DateTime.Now
                };

                List<Transaction> pending = new List<Transaction>();

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

                        recipientTransaction.Funds = convertedRate;

                        pending.Add(senderTransaction);
                        pending.Add(recipientTransaction);

                        Console.WriteLine($"Transaction created successfully.");
                        return pending;
                    }
                    else
                    {
                        Console.WriteLine($"Transaction failed. Transfer amount cannot exceed sender account balance.");
                        return pending;
                    }
                }
                catch
                {
                    Console.WriteLine("Transaction failed");
                    return pending;
                }

            }
            catch
            {
                Console.WriteLine($"Could not find the account with account number:{recipientAccID}");
                return new List<Transaction>();
            }

            return new List<Transaction>();
        }

        public void FinalizeTransaction(Transaction senderTransaction, Transaction recipientTransaction)
        {

            var userList = Login.UserList;

            User sender = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderTransaction.SenderID));
            User recipient = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == recipientTransaction.ReceiverID));
            if(sender != null && recipient != null)
            {
                // Deduct funds from sender
                sender.Accounts.First(a => a.AccountID == senderTransaction.SenderID).Funds -= senderTransaction.Funds;
                sender.Accounts.First(a => a.AccountID == senderTransaction.SenderID).AccTransList.Add(senderTransaction);
                // Add funds to recipient
                recipient.Accounts.First(a => a.AccountID == recipientTransaction.ReceiverID).Funds += recipientTransaction.Funds;
                recipient.Accounts.First(a => a.AccountID == recipientTransaction.ReceiverID).AccTransList.Add(recipientTransaction);

                //Replaces values of acounts in the userlist with updated values, using index to identify
                //their locations and overwriting the old data with the updated sender and recipient data.
                var indexSender = userList.IndexOf(sender);
                var indexRecipient = userList.IndexOf(recipient);
                if (indexSender > -1 && indexRecipient > -1)
                {
                    userList[indexSender] = sender;
                    userList[indexRecipient] = recipient;

                    Login.UserList = userList;
                }
                Console.WriteLine($"Transaction {senderTransaction.SenderID} -> {recipientTransaction.ReceiverID} confirmed!");
            }
            else
            {
                Console.WriteLine("Sender or recipient not found.");
            }

        }


        /// <summary>
        /// Executes pending transactions that have been delayed for more than 15 minutes.
        /// </summary>
        /// <remarks>This method processes transactions from the pending transaction list. It checks each
        /// transaction's elapsed time and executes it if the transaction has been pending for more than 15 minutes.
        /// Transactions are confirmed in pairs if they have the same sender, receiver, and transaction date.</remarks>
        public void ExecutePendingTransactions()
        {
            Transaction trans = new Transaction();
            List<Transaction> toBeExecuted = new List<Transaction>();

            //Checks entire list of pending transactions
            foreach (var transaction in BankingMain.pendingTransaction)
            {
                //transactionElapseTime variable functions as a check where we add 15 minutes to the saved time of the transfer and check it against the current time.
                var transactionElapseTime = transaction.TransDate.AddMinutes(15);

                //If the list is empty and more than 15 minutes have passed we add the first element to the toBeExecuted list. This element will always be the sender 
                //based on the order they are saved to the PendingTransactions property by the PrepFundsTransfer method.
                if (toBeExecuted.Count == 0 && DateTime.Now > transactionElapseTime)
                {
                    toBeExecuted.Add(transaction);
                }
                //Based on the above if statement, the Transaction object will be the recipient if the list is populated by one variable already.
                //To ensure that this is the same transaction we compare the current incoming variable to the one already saved to the list.
                //If the SenderID, RecipientID and TransDate properties of the incoming Transaction object are
                //the same as in the one found in the toBeExecuted list we know that these are the same transaction and we send them to the
                //ConfirmTransaction method to to execute the transaction and we clear the toBeExecuted list so that
                //a new transaction pair can be processed.
                else if (toBeExecuted.Count == 1 && DateTime.Now > transactionElapseTime)
                {
                    var existing = toBeExecuted.FirstOrDefault();
                    if (existing.SenderID == transaction.SenderID && existing.ReceiverID == transaction.SenderID && existing.TransDate == transaction.TransDate)
                    {
                        toBeExecuted.Add(transaction);

                        //Safety check in case an admin has manually processed the transaction while this is running
                        var safety = BankingMain.pendingTransaction.First(x => x.SenderID == transaction.SenderID  && x.ReceiverID == transaction.ReceiverID && x.TransDate == transaction.TransDate);
                        if (safety != null) 
                        {
                            trans.ConfirmTransaction(toBeExecuted[0], toBeExecuted[1]);
                            toBeExecuted.Remove(toBeExecuted[0]);
                            toBeExecuted.Remove(toBeExecuted[1]);
                        }
                    }
                }
            }
        }
    }
}
