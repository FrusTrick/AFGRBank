using AFGRBank.Utility;
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
            bool isContinue = true;
            while (isContinue)
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
            Console.Clear();

            Console.WriteLine($"Username:");
            string username = Console.ReadLine()
                .Trim();
            Console.WriteLine($"Password:");
            string password = Console.ReadLine()
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
                        return;
                }
            }
        }

        public void AdminMenu(string name, string surname)
        {
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
                        break;
                    case AdminMenuOptions.UpdateCurrencyRate:
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

        public void TransferMenu()
        {
            string senderID = "None";
            string receiverID = "None";
            decimal amount = 0;

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

            bool isContinue = true;
            while (isContinue)
            {
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
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Pick a BankingMain menu to test!" +
                    $"\n[0] Quit" +
                    $"\n[1] MainMenu()" +
                    $"\n[2] LoginMenu()" +
                    $"\n[3] UserMenu()" +
                    $"\n[4] AdminMenu()" +
                    $"\n[5] AccountMenu()" +
                    $"\n[6] TransferMenu()" +
                    $"\n[7] LoanMenu()");
            
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
                        AccountMenu();
                        break;
                    case "6":
                        TransferMenu();
                        break;
                    case "7":
                        LoanMenu();
                        break;
                }
            }
        }
    }
}
