// To get the total funds of a user when creating a loan
using AFGRBank.BankAccounts;

// For accessing the .json file that contains all exchange rates
using AFGRBank.Exchange;

// To be able to use the Loan class to initialize a loan
using AFGRBank.Loans;

// To be able to access the enum list without writing CurrencyExchange.CurrencyNames
using static AFGRBank.Exchange.CurrencyExchange;

using System.Text.Json;

using System.Text.Json.Serialization;

using AFGRBank.Utility;

namespace AFGRBank.UserType
{
    public class Admin : User
    {
        public bool IsAdmin { get; private set; } = true;

        // Calls to create a new user
        // Loops through to make sure there are no duplicate UserNames
        // If there aren't, returns a new userlist with the created user ...
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
            catch (Exception ex)
            {
                Console.WriteLine($"CreateUser failed to process: {ex.Message}");
            }
            return userList;
        }

        // TODO: Implement real currency update logic
        public void UpdateCurrencyRates(CurrencyNames currencyName, decimal updatedAmount)
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

                // Ensure file exists
                if (!File.Exists(jsonPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(jsonPath)!); // If it doesn't exist, create it
                    File.WriteAllText(jsonPath, "{}");
                }

                string jsonString = File.ReadAllText(jsonPath);

                // Debugging, feel free to remove
                /*
                Console.WriteLine("JSON read.");
                Console.WriteLine(File.Exists(jsonPath));  // Should print True
                Console.WriteLine(File.ReadAllText(jsonPath));  // Should show updated JSON
                */

                // Decodes the json file into dictionary format CurrencyName: decimal
                var currencyRates = JsonSerializer.Deserialize<Dictionary<CurrencyExchange.CurrencyNames, decimal>>(jsonString, options)
                                    ?? new Dictionary<CurrencyNames, decimal>(); // Creates a dictionary to ensure that the program doesn't crash in case the file is empty
                
                // Updates the dictionary
                currencyRates[currencyName] = updatedAmount;

                // Update the files
                File.WriteAllText(jsonPath, JsonSerializer.Serialize(currencyRates, options));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateCurrencyRates failed to process: {ex.Message}");
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
            catch (Exception ex)
            {
                Console.WriteLine($"CreateLoan failed to process: {ex.Message}");
            }
        }
    }
}
