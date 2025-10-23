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
        public bool IsAdmin { get; private set; } = true;

        // Calls to create a new user
        // Loops through to make sure there are no duplicate UserNames
        // If there aren't, returns a new userlist with the created user
        public List<User> CreateUser(List<User> userList, string username, string password, string name, string surName, int phoneNumber, string address)
        {
            try
            {
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
            }
            catch
            {
                Console.WriteLine("CreateUser failed to process information");
            }
            

            
            return userList;
        }

        // TODO: Implement real currency update logic
        public string UpdateCurrencyRates(string currency)
        {
            return currency;
        }

        public void CreateLoan(User user, Account account, decimal loanAmount, string currency, decimal interestRate, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                // GetTotalFunds returns a decimal variable of all the funds between every account
                decimal maxLoan = (user.GetTotalFunds() * 5);
                if (user.LoanList != null)
                {
                    maxLoan -= user.LoanList.Sum(l => l.LoanAmount); // Adds the sum of all LoanList LoanAmounts and subtracts it from the maxLoan
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
            catch
            {
                Console.WriteLine("CreateLoan failed to process information");
            }
        }
    }
}
