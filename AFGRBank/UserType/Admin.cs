using AFGRBank.BankAccounts;
using AFGRBank.Exchange;
using AFGRBank.Loans;
using System.Text.Json;
using System.Text.Json.Serialization;
using static AFGRBank.Exchange.CurrencyExchange;

namespace AFGRBank.UserType
{
    public class Admin : User
    {
        public bool IsAdmin { get; private set; } = true;

        // Calls to create a new user
        // Loops through to make sure there are no duplicate UserNames
        // If there aren't, returns a new userlist with the created user
        public List<User> CreateUser(string username, string password, string name, string surName, string email, int phoneNumber, string address, List<User> userList)
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
                    Address = address,
                    Email = email,
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
        public void UpdateCurrencyRates(CurrencyExchange currencyExchange, string chosenCurrency, decimal updatedAmount)
        {
            try
            {
                // Set the options to handle CurrencyName enum keys
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }, // Converts json string to Enum
                    WriteIndented = true // Essentially reformats the spaces for the json file for machine reading 
                };

                // Ensures the file path that is being updated is the currenct directory we're using and not the debugger files that vss generates
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
                string jsonString = File.ReadAllText(jsonPath);

                // Debugging, feel free to remove
                /*
                Console.WriteLine("JSON read.");
                Console.WriteLine(File.Exists(jsonPath));  // Should print True
                Console.WriteLine(File.ReadAllText(jsonPath));  // Should show updated JSON
                */

                // Decodes the json file into dictionary format CurrencyName: decimal
                var currencyRates = JsonSerializer.Deserialize<Dictionary<CurrencyName, decimal>>(jsonString, options)
                                    ?? new Dictionary<CurrencyName, decimal>(); // Creates a dictionary to ensure that the program doesn't crash in case the file is empty

                // Tries to convert string into enum, and if it's true initialize currency and change the key pair that matches currency to the updated amount
                if (Enum.TryParse<CurrencyName>(chosenCurrency, true, out var currency))
                {
                    currencyRates[currency] = updatedAmount;
                    Console.WriteLine("Updated to: " + updatedAmount + currency);
                }
                else
                {
                    Console.WriteLine("Invalid currency given...");
                }

                // Update the files
                File.WriteAllText(jsonPath, JsonSerializer.Serialize(currencyRates, options));
                Console.WriteLine("JSON updated.");
                Console.WriteLine(File.Exists(jsonPath));  // Should print True
                Console.WriteLine(File.ReadAllText(jsonPath));  // Should show updated JSON

            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateCurrencyRates Failed to process: " + ex.Message);
            }
        }

        // Create's a Loan object with given parameters
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
