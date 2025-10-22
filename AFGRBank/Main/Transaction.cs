using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Main
{
    public class Transaction
    {
        
        public int SenderID { get; set; }
        public int RecieverID { get; set; }
        public decimal Funds { get; set; }
        public DateTime TransDate { get; set; }

        public Transaction()
        {
            
        }
    }
}
