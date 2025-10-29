using AFGRBank.BankAccounts;
using AFGRBank.Exchange;
using AFGRBank.Loans;
using AFGRBank.Utility;
using AFGRBank.UserType;
using static AFGRBank.Exchange.CurrencyExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AFGRBank.Main
{
    public class BankingMain
    {
        public void MainMenu()
        {
            string asciiArt =
                "ASCII Placeholder\n" +
                "ASCII Placeholder\n" +
                "ASCII Placeholder\n";

            string[] mainMenuOptions = { "Login", "Exit" };
            while (true)
            {
                MainMenuOptions selectedOption = Menu.ReadOption<string, MainMenuOptions>(asciiArt, mainMenuOptions);
                switch (selectedOption)
                {
                    case MainMenuOptions.Login:
                        LoginMenu();
                        break;
                    case MainMenuOptions.Exit:
                        return;
                }
            }
        }

        public void LoginMenu()
        {
            int attempts = 3;
            string username = string.Empty;
            string password = string.Empty;

            string promptText = "Sign in to account:";
            string[] menuOptions = { "Username:", "Password:", "Login", "Exit" };
            while (true)
            {
                var selectedOptions = Menu.ReadOptionIndex(promptText, menuOptions);
                // LoginMenuOptions selectedOptions = Menu.ReadOption<string, LoginMenuOptions>(text, loginOptions);
                switch (selectedOptions)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        return;
                }
            }

            Console.WriteLine($"Username:");
            username = Console.ReadLine()
                .Trim();
            Console.WriteLine($"Password:");
            password = Console.ReadLine()
                .Trim();

            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine($"Username cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine($"Password cannot be empty.");
                return;
            }
            // login.LoginUser(username, password);
        }

        public void UserMenu(string name, string surname)
        {
            string text = $"Welcome {name} {surname}.";
            string[] userMenuOptions = { 
                "Borrow money", 
                "Change currency", 
                "View your bank accounts", 
                "View interest rates",
                "View transactions",
                "Logout" 
            };
            
            bool isContinue = true;
            while (isContinue)
            {
                UserMenuOptions selectedOption = Menu.ReadOption<string, UserMenuOptions>(text, userMenuOptions);
                switch (selectedOption)
                {
                    case UserMenuOptions.Borrow:
                        break;
                    case UserMenuOptions.SetCurrency:
                        break;
                    case UserMenuOptions.ViewAccounts:
                        break;
                    case UserMenuOptions.ViewInterests:
                        break;
                    case UserMenuOptions.ViewTransactions:
                        break;
                    case UserMenuOptions.Logout:
                        // login.LogoutUser();
                        return;
                }
            }
        }

        public void AdminMenu(string name, string surname)
        {
            Admin admin = new Admin();

            string text = $"Welcome {name} {surname}." +
                $"\nYou're logged in as Admin.";
            string[] adminMenuOptions = {
                "Create new user",
                "Update currency rate",
                "Borrow money",
                "Change currency",
                "View your bank accounts",
                "View interest rates",
                "View transactions",
                "Logout"
            };

            bool isContinue = true;
            while (isContinue)
            {
                AdminMenuOptions selectedOption = Menu.ReadOption<string, AdminMenuOptions>(text, adminMenuOptions);
                switch (selectedOption)
                {
                    case AdminMenuOptions.CreateUser:
                        CreateUserMenu(admin);
                        break;
                    case AdminMenuOptions.UpdateCurrencyRate:
                        // Update exchange rate for a specified currency.

                        string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
                        string jsonString = File.ReadAllText(jsonPath);

                        // The default selected currency and values (TryParse doesn't allow null CurrencyNames)
                        // These two will be used as parameter in UpdateExchangeRates method
                        CurrencyNames selectedCurrency = CurrencyNames.SEK;
                        decimal newRates = 1m;

                        // These variables will be used to display the selected currency and the new exchange rates admin inputted
                        string displaySelectedCurrency = string.Empty;
                        string displayNewRates = string.Empty;
                        
                        bool isContinue2 = true;
                        while (isContinue2 == true)
                        {
                            // Display .JSON content to string, replacing the curly brackets with whitespaces
                            string promptText = $"Select which currency's exchange rate to update:" +
                                $"\n{jsonString}"
                                .Replace('{', ' ')
                                .Replace('}', ' ');

                            // Call ReadOptionIndex with parameters declared inside instead of declaring them outside the method first
                            // This is used to update {displaySelectedCurrency} and {displayNewRates} in real time.
                            // If you hover over ReadOptionIndex, you can see it takes in two parameters:
                            //      questionTetxt is the text displayed above the menu buttons
                            //      menuOptions contain the buttons, separated with the comma character ','
                            // ReadOptionIndex returns an integer value which is used to compare the switch case below.
                            int selectUpdateRateOptions = Menu.ReadOptionIndex(
                                $"Select which currency's exchange rate to update:" +
                                $"\n{jsonString}"
                                .Replace('{', '\n')
                                .Replace('}', '\n'),
                                [ 
                                $"Select currency:        {displaySelectedCurrency}",
                                $"Set new exchange rate:  {displayNewRates}",
                                $"Update",
                                $"Exit",
                                ]);

                            switch (selectUpdateRateOptions)
                            {
                                case 0:
                                    // Admin inputs which currency to update. There's validation to ensure input matches the correct enum.
                                    Console.Clear();
                                    string inputSelectedCurrency = Validate.GetInput(
                                        $"\n{jsonString
                                        .Replace('{', '\n')
                                        .Replace('}', '\n')}" +
                                        $"Input the currency which needs to update its exchange rate:",
                                        $"Input cannot be empty, please try again.");
                                    if (!Enum.TryParse(inputSelectedCurrency, true, out selectedCurrency))
                                    {
                                        Console.WriteLine($"Currency does not exists, please try again.");
                                        break;
                                    }
                                    // displaySelectedCurrency is used to display which currency admin picked, in case they forget
                                    displaySelectedCurrency = selectedCurrency.ToString();
                                    break;
                                case 1:
                                    // Admin inputs the updated exchange rate. There's validation to ensure input is decimal.
                                    newRates = Validate.StringToDecimal(
                                        $"\n{jsonString
                                        .Replace('{', '\n')
                                        .Replace('}', '\n')}" +
                                        $"Input the updated exchange rate:",
                                        $"Input cannot be empty, please try again.",
                                        $"Input failed to parse. Make sure the input doesn't contain invalid characters and try again.");
                                    // displaySelectedCurrency is used to display the updated exchange rates the admin picked, in case they forget
                                    displayNewRates = newRates.ToString();
                                    break;
                                case 2:
                                    // Calls UpdateCurrencyRates which updates the selected currency with the new rates.
                                    // Then read .JSON file and update displayText.
                                    admin.UpdateCurrencyRates(selectedCurrency, newRates);
                                    jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
                                    jsonString = File.ReadAllText(jsonPath);
                                    break;
                                case 4:
                                    // Exits AdminMenuOptions.UpdateCurrencyRate 
                                    isContinue2 = false;
                                    break;
                            }
                        }

                        break;
                    case AdminMenuOptions.Borrow:
                        break;
                    case AdminMenuOptions.SetCurrency:
                        break;
                    case AdminMenuOptions.ViewAccounts:
                        break;
                    case AdminMenuOptions.ViewInterests:
                        break;
                    case AdminMenuOptions.ViewTransactions:
                        break;
                    case AdminMenuOptions.Logout:
                        return;
                }
            }
        }

        private void CreateUserMenu(Admin admin)
        {
            string username = string.Empty;
            string password = string.Empty;
            string name = string.Empty;
            string surname = string.Empty;
            string email = string.Empty;
            int phoneNumber = 0;
            string address = string.Empty;

            //string username = "test";
            //string password = "test";
            //string name = "test";
            //string surname = "test";
            //string email = "test@test.se";
            //int phoneNumber = 070;
            //string address = "testHome";

            bool isContinue = true;
            while (isContinue)
            {
                string text = $"User account creation:";
                string[] createUserMenuOptions = {
                    $"Edit username              Current: {username}",
                    $"Edit password              Current: {password}",
                    $"Edit name                  Current: {name}",
                    $"Edit surname               Current: {surname}",
                    $"Edit email address         Current: {email}",
                    $"Edit phone number          Current: {phoneNumber}",
                    $"Edit address               Current: {address}",
                    $"Create new user",
                    $"Exit"
                };

                CreateUserMenuOptions selectedOption = Menu.ReadOption<string, CreateUserMenuOptions>(text, createUserMenuOptions);
                switch (selectedOption)
                {
                    case CreateUserMenuOptions.EditUsername:
                        Console.Clear();
                        username = Validate.GetInput("Input new username:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditPassword:
                        Console.Clear();
                        password = Validate.GetInput("Input new password:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditName:
                        Console.Clear();
                        name = Validate.GetInput("Input new name:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditSurname:
                        Console.Clear();
                        surname = Validate.GetInput("Input new surname:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditEmail:
                        Console.Clear();
                        email = Validate.GetInput("Input new email address:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditPhoneNumber:
                        Console.Clear();
                        phoneNumber = Validate.StringToInt("Input new phone number (numbers only):", 
                            "Input cannot be empty. Try again.", "Input was not a number. Try again.", 
                            "Input was either too big or too small. Try again."
                            );
                        break;
                    case CreateUserMenuOptions.EditAddress:
                        Console.Clear();
                        address = Validate.GetInput("Input new physical address:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.CreateUser:
                        if (username == string.Empty || 
                            password == string.Empty || 
                            name == string.Empty || 
                            surname == string.Empty ||
                            email == string.Empty ||
                            phoneNumber <= 0 ||
                            address == string.Empty)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Error. One or more fields are empty." +
                                $"\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        Login login = new Login();
                        login.UserList = admin.CreateUser(username, password, name, surname, email, phoneNumber, address, login.UserList);
                        break;
                    case CreateUserMenuOptions.Exit:
                        return;
                }
            }
        }

        public void AccountMenu()
        {
            string text = $"Your bank account menu.";
            string[] accountMenuOptions = { 
                "View your account info",
                "View your account transactions",
                "Transfer funds",
                "Create account",
                "Delete account",
                "Exit" 
            };

            bool isContinue = true;
            while (isContinue)
            {
                AccountMenuOptions selectedOption = Menu.ReadOption<string, AccountMenuOptions>(text, accountMenuOptions);
                switch (selectedOption)
                {
                    case AccountMenuOptions.ViewAccountInfo:
                        break;
                    case AccountMenuOptions.ViewAccountTransactions:
                        break;
                    case AccountMenuOptions.TransferFunds:
                        break;
                    case AccountMenuOptions.CreateAccount:
                        break;
                    case AccountMenuOptions.DeleteAccount:
                        break;
                    case AccountMenuOptions.Exit:
                        return;
                }
            }
        }
        public void SavingsAccountMenu()
        {
            string text = $"Your bank account menu.";
            string[] savingAccountMenuOptions = {
                "View your account info",
                "View your account transactions",
                "View savings forecast" +
                "Transfer funds",
                "Create account",
                "Delete account",
                "Exit"
            };

            bool isContinue = true;
            while (isContinue)
            {
                SavingsAccountMenuOptions selectedOption = Menu.ReadOption<string, SavingsAccountMenuOptions>(text, savingAccountMenuOptions);
                switch (selectedOption)
                {
                    case SavingsAccountMenuOptions.ViewAccountInfo:
                        break;
                    case SavingsAccountMenuOptions.ViewAccountTransactions:
                        break;
                    case SavingsAccountMenuOptions.ViewSavingsForecast:
                        break;
                    case SavingsAccountMenuOptions.TransferFunds:
                        break;
                    case SavingsAccountMenuOptions.CreateAccount:
                        break;
                    case SavingsAccountMenuOptions.DeleteAccount:
                        break;
                    case SavingsAccountMenuOptions.Exit:
                        return;
                }
            }
        }


        public void TransferMenu()
        {
            string senderID = "None";
            string receiverID = "None";
            decimal amount = 0;

            bool isContinue = true;
            while (isContinue)
            {
                string text = $"Transfer funds:" +
                    $"\nFrom:   {senderID}" +
                    $"\nTo:     {receiverID}" +
                    $"\nAmount: {amount}";
            
                string[] transferMenuOptions = {
                    "Choose bank account to send from",
                    "Choose bank account to send to",
                    "Choose amount",
                    "Exit"
                };

                TransferMenuOptions selectedOption = Menu.ReadOption<string, TransferMenuOptions>(text, transferMenuOptions);
                switch (selectedOption)
                {
                    case TransferMenuOptions.SetSenderID:
                        senderID = "TestAccount";
                        break;
                    case TransferMenuOptions.SetReceiverID:
                        receiverID = "TestAccount";
                        break;
                    case TransferMenuOptions.SetAmount:
                        amount = 1999.99M;
                        break;
                    case TransferMenuOptions.Exit:
                        return;
                }
            }
        }

        public void LoanMenu()
        {
            string text = $"Borrow money from AFGR Bank.";
            string[] transferMenuOptions = {
                "Create loan",
                "Get loan",
                "Edit loan",
                "Exit"
            };

            bool isContinue = true;
            while (isContinue)
            {
                LoanMenuOptions selectedOption = Menu.ReadOption<string, LoanMenuOptions>(text, transferMenuOptions);
                switch (selectedOption)
                {
                    case LoanMenuOptions.CreateLoan:
                        break;
                    case LoanMenuOptions.GetLoan:
                        break;
                    case LoanMenuOptions.EditLoan:
                        break;
                    case LoanMenuOptions.Exit:
                        return;
                }
            }
        }

        public void Testing()
        {
            Admin admin = new Admin();
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Pick a BankingMain menu to test!" +
                    $"\n[0] Quit" +
                    $"\n[1] MainMenu()" +
                    $"\n[2] LoginMenu()" +
                    $"\n[3] UserMenu()" +
                    $"\n[4] AdminMenu()" +
                    $"\n[5] CreateUserMenu()" +
                    $"\n[6] AccountMenu()" +
                    $"\n[7] SavingsAccountMenu()" +
                    $"\n[8] TransferMenu()" +
                    $"\n[9] LoanMenu()");
            
                string input = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine($"Input was empty, please try again.");
                    continue;
                }
                switch (input) 
                {
                    case "0":
                        return;
                    case "1":
                        MainMenu();
                        break;
                    case "2":
                        LoginMenu();
                        break;
                    case "3":
                        UserMenu("Förnamn", "Efternamn");
                        break;
                    case "4":
                        AdminMenu("Förnamn", "Efternamn");
                        break;
                    case "5":
                        CreateUserMenu(admin);
                        break;
                    case "6":
                        AccountMenu();
                        break;
                    case "7":
                        break;
                    case "8":
                        TransferMenu();
                        break;
                    case "9":
                        LoanMenu();
                        break;
                    default:
                        Console.WriteLine($"Error. Incorrect input. Try again.");
                        break;
                }
            }
        }
    }
}
