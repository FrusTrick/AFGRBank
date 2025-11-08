using AFGRBank.Exchange;
using AFGRBank.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;
using AFGRBank.BankAccounts;

namespace AFGRBank.Main
{
    public class Transaction
    {
        public Guid SenderID { get; set; }
        public Guid ReceiverID { get; set; }
        public decimal Funds { get; set; }
        public DateTime TransDate { get; set; }

        // Initializing to access the FinalizeTransaction() method.
        PendingTransaction pTransaction = new PendingTransaction();

        public Transaction()
        {

        }

        /// <summary>
        /// Confirms a transaction by finalizing the transfer between a sender and a recipient.
        /// </summary>
        /// <param name="senderTransaction">The transaction representing the sender's side of the transfer.</param>
        /// <param name="recipientTransaction">The transaction representing the recipient's side of the transfer.</param>
        /// <remarks>
        /// Calls <see cref="PendingTransaction.FinalizeTransaction(Transaction, Transaction)"/> to apply the transfer.
        /// After finalizing, both transactions are removed from <see cref="BankingMain.pendingTransaction"/>.
        /// Ensures that both transactions are non-null before processing.
        /// </remarks>
        public void ConfirmTransaction(Transaction senderTransaction, Transaction recipientTransaction)
        {
            // If the objects null exit the method with a message.
            if (senderTransaction == null || recipientTransaction == null)
            {
                Console.WriteLine("Sender or recipient is null.");
                return;
            }

            pTransaction.FinalizeTransaction(senderTransaction, recipientTransaction);

            // Removes the same reference object as
            BankingMain.pendingTransaction.Remove(senderTransaction);
            BankingMain.pendingTransaction.Remove(recipientTransaction);

            Console.WriteLine($"Transaction {senderTransaction.SenderID} -> {recipientTransaction.ReceiverID} confirmed and removed.");
        }

        /// <summary>
        /// Removes any expired <see cref="Transaction"/> in the static <see cref="BankingMain.pendingTransaction"/> list
        /// </summary>
        /// <remarks>
        /// Searches for <see cref="Transaction"/> objects with Date's past x minutes in the 
        /// <see cref="BankingMain.pendingTransaction"/> list and saves them as a local expiredTransactions list.
        /// If found, removes them from the <see cref="BankingMain.pendingTransaction"/> list
        /// </remarks>
        public void RemoveExpiredTransactions()
        {
            // The time in minutes before a transaction expires
            double transactionTimeout = 15;

            // Find all expired transactions
            var expiredTransactions = BankingMain.pendingTransaction
                .Where(pt => (DateTime.Now - pt.TransDate) >= TimeSpan.FromMinutes(transactionTimeout))
                .ToList();

            // Loops through the static list in banking main
            foreach (var pt in expiredTransactions)
            {
                BankingMain.pendingTransaction.Remove(pt);
                Console.WriteLine($"Removed expired transaction from: {pt.SenderID} to: receiver ID: {pt.ReceiverID}");
            }

            if (expiredTransactions.Count == 0)
            {
                Console.WriteLine("No expired transactions found.");
            }
        }


        /// <summary>
        /// Displays all transactions to and from all user accounts
        /// </summary>
        /// <remarks>Will write all transctions to console and adjust message based on 
        /// the account holder being the sender or recipient of the funds.
        /// If no trasnactions are found, a message informing of that will be displayed instead</remarks>
        /// <param name="user"></param>
        public void DisplayAllTransactions(User user)
        {
            List<Transaction> allTranactions = new List<Transaction>();

            foreach (Account account in user.Accounts) 
            {
                foreach (Transaction trasnaction in account.AccTransList)
                { 
                    allTranactions.Add(trasnaction);
                }
            }

            if (allTranactions.Count > 0) {

                foreach (Transaction transaction in allTranactions)
                {
                    bool sent;
                    sent = user.Accounts.Exists(x => x.AccountID == transaction.SenderID);
                    if (sent)
                    {
                        var account = user.Accounts.Find(x => x.AccountID == transaction.SenderID);
                        Console.WriteLine($"____________________________________");
                        Console.WriteLine($"You sent {transaction.Funds} {account.Currency.ToString()} " +
                            $"\nFrom: {account.AccountID} \nTo: {transaction.ReceiverID}" +
                            $"\nTransaction Date: {transaction.TransDate.ToShortTimeString()}");
                        Console.WriteLine($"____________________________________");
                    }
                    else if (!sent)
                    {
                        var account = user.Accounts.Find(x => x.AccountID == transaction.ReceiverID);
                        Console.WriteLine($"____________________________________");
                        Console.WriteLine($"You Recieved {transaction.Funds} {account.Currency.ToString()} " +
                            $"\nFrom: {transaction.SenderID} \nTo: {account.AccountID}" +
                            $"\nTransaction Date: {transaction.TransDate.ToShortTimeString()}");
                        Console.WriteLine($"____________________________________");
                    }
                }
            }
            else
            {
                Console.WriteLine("You have no transaction history");
            }
        }
    }
}
