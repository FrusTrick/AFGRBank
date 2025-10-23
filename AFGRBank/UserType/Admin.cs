using AFGRBank.BankAccounts;
using AFGRBank.Loans;
using AFGRBank.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.UserType
{
    public class Admin : User
    {
        public bool IsAdmin { get; set; }

        // Calls to create a new user
        // Loops through to make sure there are no duplicate UserNames
        // If there aren't, returns a new userlist with the created user
        public List<User> CreateUser(List<User> userList, string username, string password, string name, string surName, int phoneNumber, string address)
        {
            if (userList.Any(u => UserName == username))
            {
                Console.WriteLine("The username is already taken.");
                return userList;
            }

            User newUser = new User
            {
                UserName = username,
                Password = password,
                Name = name,
                Surname = surName,
                PhoneNumber = phoneNumber,
                Address = address
            };

            userList.Add(newUser);
            Console.WriteLine($"{username} successfully created.");
            return userList;
        }

        public void UpdateCurrencyRates()
        {
            
        }

        public void Loan(User user, Account account, decimal loanAmount, string currency, decimal interestRate, DateOnly startDate, DateOnly endDate)
        {
            decimal maxLoan = (account.Funds * 5);
            if (user.LoanList != null)
            {
                maxLoan -= user.LoanList.Sum(l => l.LoanAmount);
            }
            if (loanAmount <= 0 || loanAmount > maxLoan)
            {
                Console.WriteLine("You are not eligible for a loan");
                return;
            }

            Loan newLoan = new Loan
            {
                Currency = currency,
                InterestRate = interestRate,
                StartDate = startDate,
                EndDate = endDate,
                LoanAmount = loanAmount
            };

            user.AddLoan(newLoan);
            Console.WriteLine($"{loanAmount} has now been sent to your account with an interest rate of {interestRate}.");
        }
    }
}
