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

        //// Async method that initializes a Task and delays it from acting for x minutes and then calls on Confirm();
        //public async Task StartCountdown(int minutes = 15)
        //{
        //    await Task.Delay(TimeSpan.FromMinutes(minutes));
        //    if (!Confirmed)
        //    {
        //        FinalizeTransaction();
        //    }
        //}
    }
}
