// To get the total funds of a user when creating a loan
using AFGRBank.BankAccounts;
// For accessing the .json file that contains all exchange rates
using AFGRBank.Exchange;
// To be able to use the Loan class to initialize a loan
using AFGRBank.Loans;
using AFGRBank.Main;
using AFGRBank.Utility;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Serialization;
// To be able to access the enum list without writing CurrencyExchange.CurrencyNames
using static AFGRBank.Exchange.CurrencyExchange;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AFGRBank.UserType
{
    public class Admin : User
    {
        public bool IsAdmin { get; set; } = true;

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
        public void CreateLoan(User user, Account account, decimal loanAmount, CurrencyNames currency, decimal interestRate)
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

                decimal monthlyPayment = loanAmount * 0.05m;
                decimal monthlyInterest = interestRate / 12m;

                int months = (int)Math.Ceiling(
                    Math.Log((double)(monthlyPayment / (monthlyPayment - loanAmount * monthlyInterest))) /
                    Math.Log((double)(1 + monthlyInterest))
                );

                Loan newLoan = new Loan();
                newLoan.CreateLoan(currency, interestRate, loanAmount, months);
                user.AddLoan(newLoan);
                Console.WriteLine($"{loanAmount} has now been sent to your account with an interest rate of {interestRate}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateLoan failed to process: {ex.Message}");
            }
        }

        // Provides the admin a list of all pending transactions and allows them to view & confirm them
        public void ViewPendingTransactions()
        {
            string promptText = "Choose a transaction to confirm or exit";
            List<string> menuOptions = new List<string>(); // For saving the menu options
            List<PendingTransaction> menuTransactions = new List<PendingTransaction>(); // For saving the pending transactions

            // Build the list of pending transactions
            foreach (var pt in BankingMain.PTransaction.Where(t => !t.Confirmed))
            {
                menuOptions.Add(
                    $"From: {pt.CurrentSender.UserName} -> To: {pt.CurrentReceiver.UserName}," +
                    $"\nAmount: {pt.CurrentTransaction.Funds}, \nCreated: {pt.InitializedDate}\n"
                );
                menuTransactions.Add(pt);
            }

            menuOptions.Add("Exit");

            while (true)
            {
                // Promp the ReadOptionIndexList 
                int selectedIndex = Menu.ReadOptionIndexList(promptText, menuOptions); 
                var chosenOption = menuOptions[selectedIndex];

                if (chosenOption == "Exit")
                {
                    return;
                }
                
                if (selectedIndex < menuTransactions.Count)
                {
                    PendingTransaction selectedTransaction = menuTransactions[selectedIndex];
                    Console.Clear();
                    Console.WriteLine(
                        $"You selected transaction: \n" +
                        $"From: {selectedTransaction.CurrentSender.UserName}" +
                        $"To: {selectedTransaction.CurrentReceiver.UserName}" +
                        $"Amount: {selectedTransaction.CurrentTransaction.Funds}" +
                        $"Created: {selectedTransaction.InitializedDate}"
                    );

                    Console.WriteLine("\nDo you want to confirm this transaction early? y/n");
                    string input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "y")
                    {
                        selectedTransaction.Confirm();
                        Console.WriteLine("Transaction has been confirmed.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Transaction not confirmed, returning to list...");
                        continue;
                    }

                    Console.ReadKey();
                }
            }
            
        }

        // Confirms a pending transaction through sender and receiver IDs
        public void ConfirmTransaction(Guid senderID, Guid receiverID)
        {
            var pending = BankingMain.PTransaction.FirstOrDefault(t => t.CurrentTransaction.SenderID == senderID && t.CurrentTransaction.ReceiverID == receiverID && !t.Confirmed);

            if (pending != null)
            {
                pending.Confirm();
            }
        }
    }
}
