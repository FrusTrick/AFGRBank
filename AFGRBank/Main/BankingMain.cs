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
using System.Diagnostics;

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
        Account account = new Account();
        CheckingsAccount cAccount = new CheckingsAccount();
        SavingsAccount sAccount = new SavingsAccount();
        Transaction transaction = new Transaction();
        Loan loan = new Loan();
        
        public static List<PendingTransaction> PTransaction { get; set; } = new();

        // The first screen user will encounter, contains the options to login or exit program.
        // "loginAttempts" will be used in LoginMenu()
        public void MainMenu(short loginAttempts)
        {
            string asciiArt =
                "ASCII Placeholder\n" +
                "ASCII Placeholder\n" +
                "ASCII Placeholder\n";

            string[] mainMenuOptions = { "Login", "Exit" };

            while (true)
            {
                var selectedOption = Menu.ReadOptionIndex(asciiArt, mainMenuOptions);

                switch (selectedOption)
                {
                    case 0:
                        LoginMenu(loginAttempts);
                        return;
                    case 1:
                        Process.GetCurrentProcess().Kill();
                        return;
                }
            }
        }

        // Login screen, here user can input their username and password, as well as try to sign in or exit back to MainMenu()
        // User has a certain amount of tries to login as defined by the "loginAttempts" parameter
        public void LoginMenu(short loginAttempts)
        {
            string username = string.Empty;
            string password = string.Empty;

            while (true)
            {
                string questionText = "Sign in to bank";

                string[] menuOptions = { 
                    $"Username: {username}", 
                    $"Password: {password}", 
                    "Login", 
                    "Exit" 
                };

                var selectedOptions = Menu.ReadOptionIndex(questionText, menuOptions);

                switch (selectedOptions)
                {
                    case 0:
                        Console.Clear();
                        username = Validate.GetInput($"Username:", $"Username cannot be empty. Try again.");
                        break;

                    case 1:
                        Console.Clear();
                        password = Validate.GetInput($"Password:", $"Password cannot be empty. Try again.");
                        break;

                    case 2:
                        Console.Clear();

                        // Log in button, if user attempts to login without filling in username or password,
                        // they'll lose 1 attempt, an error message is displayed, and they're forced to retry.
                        // If loginAttempts reaches 0, they're automatically exited out of program.
                        if (username == string.Empty || password == string.Empty)
                        {
                            loginAttempts--;
                            if (loginAttempts <= 0)
                            {
                                Console.WriteLine($"Failed all login attempts. You will now be exited out of the program.");
                                Console.ReadKey();
                                Process.GetCurrentProcess().Kill();
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

                        // When both are filled, this method will try to locate an User object with matching username and password in Login.UserList
                        // and try to set that User as Login.LoggedInUser
                        login.LoginUser(username, password);

                        // Login.LoggedInUser is still "null", that means User with the matching username/password could not be found
                        // which counts as a failed login attempt. They'll lose 1 attempt, an error message is displayed, and they're forced to retry.
                        // If loginAttempts reaches 0, they're automatically exited out of program.
                        if (login.LoggedInUser == null)
                        {
                            loginAttempts--;
                            if (loginAttempts <= 0)
                            {
                                Console.WriteLine($"Failed all login attempts. You will now be exited out of the program.");
                                Console.ReadKey();
                                Process.GetCurrentProcess().Kill();
                            }
                            Console.WriteLine("Failed to login. Username or password was wrong.");
                            Console.WriteLine($"{loginAttempts} tries left. Press any key to retry...");
                            Console.ReadKey();
                            continue;
                        }
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
                var selectedOption = Menu.ReadOptionIndex(text, userMenuOptions);
                switch (selectedOption)
                {
                    case 0:
                        BorrowMenu();
                        break;
                    case 1:
                        break;
                    case 2:
                        AccountMenu();
                        break;
                    case 3:
                        user.ViewAllTransactions();
                        break;
                    case 4:
                        user.ViewAllTransactions();
                        break;
                    case 5:
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
                "Create new loan",
                "View pending transactions",
                "Logout"
            };

            bool isContinue = true;
            while (isContinue)
            {
                var selectedOption = Menu.ReadOptionIndex(text, adminMenuOptions);
                switch (selectedOption)
                {
                    case 0:
                        // Create a new User and insert that User to Login.UserList
                        CreateUserMenu();
                        break;
                    case 1:
                        // Update exchange rate for a specified "CurrencyExchange.CurrencyNames Currency"
                        UpdateCurrencyRatesMenu();
                        break;
                    case 2:
                        // Create a new loan for a specific User and put it inside their loan history (User.LoanList)
                        CreateLoanMenu();
                        break;
                    case 3:
                        // View a list of all pending transaction between Users in the system. 
                        admin.ViewPendingTransactions();
                        break;
                    case 4:
                        login.LogoutUser();
                        return;
                }
            }
        }


        

        public void AccountMenu()
        {
            string questionText = $"Your bank account menu.";
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
                var selectedOption = Menu.ReadOptionIndex(questionText, accountMenuOptions);
                switch (selectedOption)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        TransferMenu();
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
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
                var selectedOption = Menu.ReadOptionIndex(text, savingAccountMenuOptions);
                switch (selectedOption)
                {
                    case 0:
                        sAccount.ViewAccountInfo();
                        break;
                    case 1:
                        break;
                    case 2:
                        sAccount.SavingsForecast();
                        break;
                    case 3:
                        TransferMenu();
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
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
            if (login.LoggedInUser is Admin)
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
            string email = "ree@se.se";
            int phonenumber = 777777;
            string address = "Address";
            login.UserList = admin.CreateUser(username, password, name, surname, email, phonenumber, address, login.UserList);

            User seedUser = login.UserList[0];
            cAccount.CreateAccount(seedUser.Accounts, CurrencyNames.DKK);             

            admin.UserName = "admin";
            admin.Password = "1";
            admin.Name = "Mr.";
            admin.Surname = "Admin";
            admin.Email = "ad@min.se";
            admin.PhoneNumber = 666;
            admin.Address = "Address";
            admin.IsAdmin = true;
            login.UserList.Add(admin);
            
            sAccount.CreateAccount(seedUser.Accounts, CurrencyNames.USD);


            PendingTransaction pending = new(transaction, login.UserList[0], login.UserList[1]);
            PTransaction.Add(pending);
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
