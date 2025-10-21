using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Loans
{
    public class Loan
    {
        public string Currency { get; set; }
        public decimal InterestRate  { get; set; }
        DateOnly StartDate { get; set; }
        DateOnly EndDate { get; set; }
        public int LoanAmount { get; set; }

        public Loan()
        {
        }

        public void CreateLoan()
        {

        }
        public void GetLoan()
        {
        }
        public void EditLoan()
        {

        }
        public void CalcMaxLoan()
        {
        }

    }
}
