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

        /// <summary>
        /// Creates a new <see cref="User"/> object and adds it to the user list.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="name">The first name of the user.</param>
        /// <param name="surName">The last name of the user.</param>
        /// <param name="email">The email of the user in user@mail.xx format.</param>
        /// <param name="phoneNumber">The phonenumber of the user as an integer.</param>
        /// <param name="address">The address of the user.</param>
        /// <param name="userList">A list of <see cref="User"/> objects to which the new user will be added.</param>
        /// <returns>
        /// The updated list of <see cref="User"/> objects including the newly created user.
        /// </returns>
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

        /// <summary>
        /// Updates the local JSON file <c>CurrencyRates.json</c> with a new exchange rate.
        /// </summary>
        /// <param name="currencyName">The currency to update, provided as a <see cref="CurrencyNames"/> enum value.</param>
        /// <param name="updatedAmount">The new exchange rate for the specified currency.</param>
        /// <remarks>
        /// Ensures the exchange rate file exists before updating it.
        /// </remarks>
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

        /// <summary>
        /// Creates a <see cref="Loan"/> object, adds it to the <see cref="User.LoanList"/> and pays out the funds.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object to create a loan for.</param>
        /// <param name="account">The <see cref="Account"/> object to send the funds to.</param>
        /// /// <param name="loanAmount">The amount of funds the user is loaning.</param>
        /// /// <param name="currency">The currency as a <see cref="CurrencyNames"/> enum value.</param>
        /// /// <param name="interestRate">The interest rate of the loan.</param>
        /// <remarks>
        /// Loan eligibility is calculated by calling on the .GetTotalFunds() method and multiplying it by five, minus any existing loan balances.
        /// If approved, the repayment period is calculated automatically based on the loan amount and interest rate.
        /// </remarks>
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

                // Equation for calculating the monthly payment, Log(1 + ( Log(m / (m - l * mi) ) )
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

        /// <summary>
        /// Display all pending transactions for the <see cref="Admin"/> to review/confirm.
        /// </summary>
        /// <remarks>
        /// Calls on the <see cref="Menu.ReadOptionIndexList()"/> method to list all of the available transactions.
        /// When a transaction is chosen, confirm if the admin wants to confirm/decline it.
        /// </remarks>
        public void ViewPendingTransactions(List<Transaction> pending)
        {
            string promptText = "Choose a transaction to confirm or exit";
            List<string> menuOptions = new List<string>(); // For saving the menu options
            List<Transaction> menuTransactions = new List<Transaction>(); // For saving the transactions as a menu option

            // Build the list of transactions
            foreach (var pt in pending)
            {
                var sender = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == pt.SenderID));
                var recipient = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == pt.ReceiverID));
                menuOptions.Add(
                    $"From: {sender.UserName} -> To: {recipient.UserName}\n" +
                    $"Amount: {pt.Funds}\nCreated: {pt.TransDate}\n"
                );
                menuTransactions.Add(pt);
            }

            menuOptions.Add("Exit");

            while (true)
            {
                // Prompt the ReadOptionIndexList 
                int selectedIndex = Menu.ReadOptionIndexList(promptText, menuOptions); 
                var chosenOption = menuOptions[selectedIndex];

                // If exit is chosen, exit the method
                if (chosenOption == "Exit")
                {
                    return;
                }

                // Depending on the chosen index, check the matching transaction in the list
                if (selectedIndex < menuTransactions.Count)
                {
                    Transaction selectedTransaction = menuTransactions[selectedIndex];
                    var sender = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == selectedTransaction.SenderID));
                    var recipient = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == selectedTransaction.ReceiverID));
                    Console.Clear();
                    Console.WriteLine(
                        $"You selected transaction: \n" +
                        $"From: {sender.UserName}\n" +
                        $"To: {recipient.UserName}\n" +
                        $"Amount: {selectedTransaction.Funds}\n" +
                        $"Created: {selectedTransaction.TransDate}\n"
                    );

                    Console.WriteLine("\nDo you want to confirm this transaction early? y/n");
                    string input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "y")
                    {
                        PendingTransaction pendingTransaction = new(pending);
                        pendingTransaction.FinalizeTransaction(pending[0], pending[1]); // 0 refers to the sender, 1 refers to the recipient
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

        /* Redundant method, ViewPendingTransactions takes care of it.
        /// <summary>
        /// Confirms a <see cref="PendingTransaction"/> by matching the sender and receiver IDs.
        /// </summary>
        /// <param name="senderID">The unique identifier of the account sending the funds.</param>
        /// <param name="receiverID">The unique identifier of the account receiving the funds.</param>
        /// <remarks>
        /// Searches for the <see cref="PendingTransaction"/> with matching <paramref name="senderID"/> and <paramref name="receiverID"/>.
        /// If found, calls the <see cref="PendingTransaction.Confirm()"/> method to finalize the transaction.
        /// </remarks>
        public void ConfirmTransaction(Guid senderID, Guid receiverID)
        {
            var pending = BankingMain.PTransaction.FirstOrDefault(t => t.CurrentTransaction.SenderID == senderID && t.CurrentTransaction.ReceiverID == receiverID && !t.Confirmed);

            if (pending != null)
            {
                pending.FinalizeTransaction();
            }
        }
        */
    }
}
