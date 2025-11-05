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
        public Guid RecieverID { get; set; }
        public decimal Funds { get; set; }
        public DateTime TransDate { get; set; }

        public Transaction()
        {
        }

    }

    public class PendingTransaction : Transaction
    {
        public Transaction transaction { get; set; }
        public bool Confirmed { get; set; } = false;
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public PendingTransaction()
        {
        }

        public async Task StartCountdown(int minutes = 15)
        {
            await Task.Delay(TimeSpan.FromMinutes(minutes));
            ConfirmTransaction();
        }

        public void ConfirmTransaction()
        {

            if (!Confirmed)
            {
                Confirmed = true;
                Console.WriteLine("Transaction has been confirmed.");
            }
        }

        public void CancelTransaction()
        {
            if (!Confirmed)
            {
                Console.WriteLine("Transaction has been cancelled due to timeout.");
            }
        }
    }
}
