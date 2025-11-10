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
        public bool Sender {  get; set; }
        public DateTime TransDate { get; set; }
        public CurrencyExchange.CurrencyNames Currency { get; set; }

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
            double transactionTimeout = 15;

            // Make a copy of the pending transactions to avoid modifying the collection while iterating
            var pending = BankingMain.pendingTransaction;

            // Loop through pairs (sender at i, receiver at i+1)
            for (int i = 0; i < pending.Count; i += 2)
            {
                if (i + 1 >= pending.Count) break; // Safety check for an unmatched transaction

                var senderTx = pending[i];
                var receiverTx = pending[i + 1];

                bool senderExpired = (DateTime.Now - senderTx.TransDate) >= TimeSpan.FromMinutes(transactionTimeout);
                bool receiverExpired = (DateTime.Now - receiverTx.TransDate) >= TimeSpan.FromMinutes(transactionTimeout);

                // If either in the pair is expired, remove both
                if (senderExpired || receiverExpired)
                {
                    Console.WriteLine($"Removed expired transaction from: {senderTx.SenderID} to receiver ID: {receiverTx.ReceiverID}");

                    // Remove both from the original list
                    ConfirmTransaction(pending[i], pending[i + 1]);

                    i -= 2; // step back so loop continues correctly (because we removed 2 items)
                }
            }

            if (!pending.Any(pt => (DateTime.Now - pt.TransDate) >= TimeSpan.FromMinutes(transactionTimeout)))
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
            var allTransactions = user.Accounts
                .SelectMany(a => a.AccTransList)
                .Distinct()
                .ToList();

            if (allTransactions.Count == 0)
            {
                Console.WriteLine("You have no transaction history.");
                return;
            }

            foreach (var transaction in allTransactions)
            {
                bool isSent = transaction.Sender;
                var account = user.Accounts.FirstOrDefault(a =>
                    a.AccountID == (isSent ? transaction.SenderID : transaction.ReceiverID));

                if (account == null)
                    continue; // Skip if no matching account is found

                Console.WriteLine("____________________________________");
                if (isSent)
                {
                    Console.WriteLine(
                        $"You sent {transaction.Funds.ToString("0.00")} {account.Currency}\n" +
                        $"From: {account.AccountID}\n" +
                        $"To: {transaction.ReceiverID}\n" +
                        $"Transaction Date: {transaction.TransDate:d}");
                }
                else
                {
                    Console.WriteLine(
                        $"You received {transaction.Funds.ToString("0.00")} {account.Currency}\n" +
                        $"From: {transaction.SenderID}\n" +
                        $"To: {account.AccountID}\n" +
                        $"Transaction Date: {transaction.TransDate:d}");
                }
                Console.WriteLine("____________________________________");
            }
        }
    }
    
}
