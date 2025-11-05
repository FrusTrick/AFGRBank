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
    public partial class BankingMain
    {
        // Every class should be instanced here (except the classes inside the Utility folder??)
        //
        // "user" is the blueprint for creating user accounts.
        // "admin" is the blueprint for creating admin accounts.
        // "login" contains a list of every user, as well as the information of the current signed in user.
        // "cx" contains every currency and their exchange rates.
        // "cAccount" is the blueprint for a checkings bank account.
        // "sAccount" is the blueprint for a checkings bank account.
        // "transaction" is used create a new transaction between users, and save it to their history
        // "pendingT" is used for async every 15 minutes.
        // "loan" is used to create a new bank loan, and save it to user history

        User user = new User();
        Admin admin = new Admin();
        Login login = new Login();
        CurrencyExchange cx = new CurrencyExchange();
        CheckingsAccount cAccount = new CheckingsAccount();
        SavingsAccount sAccount = new SavingsAccount();
        Transaction transaction = new Transaction();
        PendingTransaction pendingT = new PendingTransaction();
        Loan loan = new Loan();



        // The first screen, contains the options to login or exit program.
        // "loginAttempts"
        public void MainMenu(short loginAttempts)
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
                        LoginMenu(loginAttempts);
                        break;
                    case MainMenuOptions.Exit:
                        return;
                }
            }
        }

        // Login screen, here user can input their username and password, as well as try to sign in or exit back to MainMenu()
        public void LoginMenu(short loginAttempts)
        {
            string username = string.Empty;
            string password = string.Empty;

            while (true)
            {
                string promptText = "Sign in to bank";
                string[] menuOptions = { 
                    $"Username: {username}", 
                    $"Password: {password}", 
                    "Login", 
                    "Exit" 
                };
                var selectedOptions = Menu.ReadOptionIndex(promptText, menuOptions);
                switch (selectedOptions)
                {
                    case 0:
                        Console.Clear();
                        username = Validate.GetInput($"Username:",
                                        $"Username cannot be empty. Try again.");
                        break;
                    case 1:
                        Console.Clear();
                        password = Validate.GetInput($"Password:",
                                        $"Password cannot be empty. Try again.");
                        break;
                    case 2:
                        Console.Clear();
                        // Log in button, if user attempts to login without filling in username or password,
                        // they lose 1 attempt, and an error message is displayed.
                        // If attempts reaches 0, they're automatically exited out of program.
                        if (username == string.Empty || password == string.Empty)
                        {
                            loginAttempts--;
                            if (loginAttempts <= 0)
                            {
                                Console.WriteLine($"Failed to login.");
                                Console.ReadKey();
                                Environment.FailFast("Shit!");
                            }
                            if (username == string.Empty)
                            {
                                Console.WriteLine($"Invalid login. Username is empty. Press any key to retry.");
                                Console.WriteLine($"{loginAttempts} tries left. Press any key to retry...");
                            }
                            else if (password == string.Empty)
                            {
                                Console.WriteLine($"Invalid login. Password is empty. Press any key to retry.");
                                Console.WriteLine($"{loginAttempts} tries left. Press any key to retry...");
                            }
                            Console.ReadKey();
                            break;
                        }

                        // If filled, calls this method which will try to locate user with matching username and password in Login.UserList
                        login.LoginUser(username, password);
                        if (login.LoggedInUser == null)
                        {
                            loginAttempts--;
                            if (loginAttempts <= 0)
                            {
                                Console.WriteLine($"Failed to login.");
                                Console.ReadKey();
                                Environment.FailFast("Shit!");
                            }
                            // If no matching user could be found, this error message will be displayed, and then resets this loop
                            Console.WriteLine("Failed to login. Username or password was wrong.");
                            Console.WriteLine($"{loginAttempts} tries left. Press any key to retry...");
                            Console.ReadKey();
                            continue;
                        }
                        Console.WriteLine($"{login.LoggedInUser.UserName} + {login.LoggedInUser.Password}");
                        Console.ReadKey();
                        return;
                    case 3:
                        return;
                }
            }
        }

        public void UserMenu()
        {
            string text = $"Welcome {login.LoggedInUser.Name} {login.LoggedInUser.Surname}.";
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
                        AccountMenu();
                        break;
                    case UserMenuOptions.ViewInterests:
                        break;
                    case UserMenuOptions.ViewTransactions:
                        break;
                    case UserMenuOptions.Logout:
                        login.LogoutUser();
                        return;
                }
            }
        }

        public void AdminMenu()
        {
            string text = $"Welcome {login.LoggedInUser.Name} {login.LoggedInUser.Surname}." +
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
                        // Create a new user and add it to Login.UserList
                        CreateUserMenu();
                        break;
                    case AdminMenuOptions.UpdateCurrencyRate:
                        // Update exchange rate for a specified currency.
                        UpdateCurrencyRatesMenu();

                        break;
                    case AdminMenuOptions.Borrow:
                        BorrowMenu();
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
                        login.LogoutUser();
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






        public bool GetIsAdmin()
        {
            if (login.LoggedInUser.IsAdmin == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetLoggedInUser()
        {
            if (login.LoggedInUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Test method for populating UserList
        public void PopulateList()
        {
            string username = "1";
            string password = "1";
            string name = "Ax";
            string surname = "Be";
            string email = "admin@se.se";
            int phonenumer = 777777;
            string address = "Address";
            login.UserList = admin.CreateUser(username, password, name, surname, email, phonenumer, address, login.UserList);
        }



        public void Testing(short loginAttempt)
        {
            PopulateList();
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
                        MainMenu(loginAttempt);
                        break;
                    case "2":
                        LoginMenu(loginAttempt);
                        Console.WriteLine($"{login.LoggedInUser.UserName} + {login.LoggedInUser.Password}");
                        Console.ReadKey();
                        break;
                    case "3":
                        UserMenu();
                        break;
                    case "4":
                        AdminMenu();
                        break;
                    case "5":
                        CreateUserMenu();
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
