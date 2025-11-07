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
        public Transaction CurrentTransaction { get; set; }
        public bool Confirmed { get; set; } = false;
        public User CurrentSender { get; set; }
        public User CurrentReceiver { get; set; }
        public DateTime InitializedDate { get; set; }

        public PendingTransaction(Transaction transaction, User sender, User receiver)
        {
            CurrentTransaction = transaction;
            CurrentSender = sender;
            CurrentReceiver = receiver;
            InitializedDate = DateTime.Now;
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
        public async Task<List<User>> TransferFunds(List<User> userList, Guid senderAccID, Guid recipientAccID, decimal funds)
        {

            try
            {
                //Find both sender account and recipient account.
                var sender = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderAccID));
                var recipient = userList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == recipientAccID));

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



                        //sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).Funds -= funds;
                        //sender.Accounts.FirstOrDefault(x => x.AccountID == senderAccID).AccTransList.Add(currentTransaction);

                        ////currentTransaction updated with recipient currency value to reflect the recieved funds correctly in accordance with the recipient currency.
                        //currentTransaction.Funds = convertedRate;
                        //recipient.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID).Funds += convertedRate;
                        //recipient.Accounts.FirstOrDefault(x => x.AccountID == recipientAccID).AccTransList.Add(currentTransaction);

                        ////Replaces values of acounts in the userlist with updated values, using index to identify
                        ////their locations and overwriting the old data with the updated sender and recipient data.
                        //var indexSender = userList.IndexOf(sender);
                        //var indexRecipient = userList.IndexOf(recipient);
                        //if (indexSender > -1 && indexRecipient > -1)
                        //{
                        //    userList[indexSender] = sender;
                        //    userList[indexRecipient] = recipient;
                        //}

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
        public void FinalizeTransaction()
        {
            if (!Confirmed)
            {
                Confirmed = true;

                // Matches the AccountID of the current sender & receiver with the current sender/receiver and changes their funds accordingly
                CurrentSender.Accounts.First(a => a.AccountID == CurrentTransaction.SenderID).Funds -= CurrentTransaction.Funds;
                CurrentReceiver.Accounts.First(a => a.AccountID == CurrentTransaction.ReceiverID).Funds += CurrentTransaction.Funds;

                // Same as above except adds the transaction to their transaction list
                CurrentSender.Accounts.First(a => a.AccountID == CurrentTransaction.SenderID).AccTransList.Add(CurrentTransaction);
                CurrentReceiver.Accounts.First(a => a.AccountID == CurrentTransaction.ReceiverID).AccTransList.Add(CurrentTransaction);

                // Confirmation line
                Console.WriteLine($"Transaction {CurrentTransaction.SenderID} -> {CurrentTransaction.ReceiverID} confirmed!");
            }
        }

        // Async method that initializes a Task and delays it from acting for x minutes and then calls on Confirm();
        public async Task StartCountdown(int minutes = 15)
        {
            await Task.Delay(TimeSpan.FromMinutes(minutes));
            if (!Confirmed)
            {
                FinalizeTransaction();
            }
        }
    }
}
