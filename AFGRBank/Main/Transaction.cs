using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.UserType;

namespace AFGRBank.Main
{
    public class Transaction
    {
        public Guid SenderID { get; set; }
        public Guid ReceiverID { get; set; }
        public decimal Funds { get; set; }
        public DateTime TransDate { get; set; }

        public Transaction()
        {

        }

    }

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

        public void Confirm()
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
                Confirm();
            }
        }
    }
}
