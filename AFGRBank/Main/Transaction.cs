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

    public class PendingTransaction 
    {
        
    }
}
