using AFGRBank.Exchange;
using AFGRBank.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;

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
}
