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

        // Initializing to access the RemoveExpiredTransactions() method.
        Transaction transaction = new Transaction();

        /// <summary>
        /// Adds funds to a user's account.
        /// </summary>
        /// <param name="user">The user receiving the funds.</param>
        /// <param name="account">The account to credit.</param>
        /// <param name="amount">The amount to add.</param>
        public void AddFunds(User user, Account account, decimal amount)
        {
            try
            {
                if (user == null || account == null)
                {
                    Console.WriteLine("User or account not found");
                    return;
                }

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be greater than zero.");
                    return;
                }

                account.Funds += amount;
                Console.WriteLine($"{amount} has been added to {user.UserName}'s account ({account.AccountID}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddFunds failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes funds from a user's account, if sufficient balance exists.
        /// </summary>
        /// <param name="user">The user whose funds will be removed.</param>
        /// <param name="account">The account to debit.</param>
        /// <param name="amount">The amount to remove.</param>
        public void RemoveFunds(User user, Account account, decimal amount)
        {
            try
            {
                if (user == null || account == null)
                {
                    Console.WriteLine("User or account not found");
                    return;
                }

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be greater than zero.");
                    return;
                }

                if (account.Funds < amount)
                {
                    Console.WriteLine("Insufficient funds.");
                    return;
                }

                account.Funds -= amount;
                Console.WriteLine($"{amount} has been removed from {user.UserName}'s account ({account.AccountID}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Funds failed: {ex.Message}");
            }
        }

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
                // Checks if username already exists in Login.UserList
                if (userList.Count != 0)
                {
                    var check = userList.First(x => x.UserName == username);
                    if (check != null)
                    {
                        Console.WriteLine($"User creation failed. \"{username}\" is an already existing user.");
                        return userList;
                    }
                }
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
                    Converters = { new JsonStringEnumConverter() }, // Converts JSON string to Enum
                    WriteIndented = true // Reformats the spaces for the JSON file for optimized machine reading
                };
                
                // Ensures the file path that is being updated is the currenct directory we're using and not the debugger files that vss generates
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");

                // Ensure file exists
                if (!File.Exists(jsonPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(jsonPath)!); // If it doesn't exist, create it
                    File.WriteAllText(jsonPath, "{}");
                }

                // Read the JSON file
                string jsonString = File.ReadAllText(jsonPath);
                
                /*
                // Debugging
                Console.WriteLine("JSON read.");
                Console.WriteLine(File.Exists(jsonPath));  // Should print True
                Console.WriteLine(File.ReadAllText(jsonPath));  // Should show updated JSON
                */

                // Decodes the JSON file into dictionary format CurrencyName: decimal
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
        /// <param name="loanAmount">The amount of funds the user is loaning.</param>
        /// <param name="currency">The currency as a <see cref="CurrencyNames"/> enum value.</param>
        /// <param name="interestRate">The interest rate of the loan.</param>
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

                decimal monthlyInterest = interestRate / 12m;

                // Simple monthly payment (you can adjust formula if you want higher/lower payments)
                decimal monthlyPayment = loanAmount * monthlyInterest / (1 - (decimal)Math.Pow((double)(1 + monthlyInterest), -12)); // for 12 months

                if (monthlyPayment <= 0 || monthlyInterest < 0)
                    throw new ArgumentException("Monthly payment and interest must be positive");

                decimal remainingBalance = loanAmount;
                int months = 0;

                // Repayment loop (safe, cannot produce negative months)
                while (remainingBalance > 0)
                {
                    remainingBalance = remainingBalance * (1 + monthlyInterest) - monthlyPayment;
                    months++;

                    if (months > 120000) // Safety limit
                        throw new ArgumentOutOfRangeException(nameof(months), "Loan cannot be repaid with current monthly payment.");
                }

                Loan newLoan = new Loan();
                newLoan.CreateLoan(currency, interestRate, loanAmount, months);
                user.AddLoan(newLoan);
                AddFunds(user, account, loanAmount);
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
            // Removes all expired transactions
            transaction.RemoveExpiredTransactions();

            // Initializing the menu
            string promptText = "Choose a transaction to confirm or exit";
            List<string> menuOptions = new List<string>(); // For saving the menu options
            List<(Transaction senderTx, Transaction receiverTx)> menuTransactions = new List<(Transaction, Transaction)>(); ; // For saving the transactions as a menu option

            for (int i = 0; i < pending.Count; i += 2)
            {

                if (i + 1 >= pending.Count) break;

                var senderTx = pending[i];
                var receiverTx = pending[i + 1];

                var sender = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderTx.SenderID));
                var recipient = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == receiverTx.ReceiverID));

                menuOptions.Add(
                    $"From: {sender?.UserName} -> To: {recipient?.UserName}\n" +
                    $"Amount: {senderTx.Funds.ToString("0.00")} {senderTx.Currency} -> {receiverTx.Funds.ToString("0.00")} {receiverTx.Currency}\n" +
                    $"Created: {senderTx.TransDate}\n"

                );

                menuTransactions.Add((senderTx, receiverTx));
            }

            // Adds the Exit option
            menuOptions.Add("Exit");

            // Loop for error handling and repetitive inputs
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
                    var (senderTx, receiverTx) = menuTransactions[selectedIndex];

                    var sender = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == senderTx.SenderID));
                    var recipient = Login.UserList.FirstOrDefault(x => x.Accounts.Any(y => y.AccountID == receiverTx.ReceiverID));

                    Console.Clear();
                    Console.WriteLine(
                        $"You selected transaction: \n" +
                        $"From: {sender?.UserName}\n" +
                        $"To: {recipient?.UserName}\n" +
                        $"Amount: {senderTx.Funds.ToString("0.00")} {senderTx.Currency} -> {receiverTx.Funds.ToString("0.00")} {receiverTx.Currency}\n" +
                        $"Created: {senderTx.TransDate}\n"
                    );

                    Console.WriteLine("\nDo you want to confirm this transaction early? y/n");
                    string? input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "y")
                    {
                        PendingTransaction pendingTransaction = new();
                        transaction.ConfirmTransaction(senderTx, receiverTx);
                        //pendingTransaction.FinalizeTransaction(pending[0], pending[1]); 
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

        // Log funktion for using decimals instead of 
        private static decimal DecimalLn(decimal x, int precision = 50)
        {
            if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x), "x must be positive.");

            decimal result = 0;
            decimal y = (x - 1) / (x + 1);
            decimal yPower = y;
            for (int n = 1; n <= precision; n += 2)
            {
                result += yPower / n;
                yPower *= y * y;
            }
            return 2 * result;
        }

        private static decimal DecimalCeiling(decimal value)
        {
            // Get the integer part
            decimal intPart = Math.Truncate(value);
            // If value has any fractional part, round up
            if (value > intPart)
                return intPart + 1;
            return intPart;
        }
    }
}
