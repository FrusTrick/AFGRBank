using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.BankAccounts
{
    public class SavingsAccount : Account
    {
        public bool isSavings { get; set; } = true;
    }
}
