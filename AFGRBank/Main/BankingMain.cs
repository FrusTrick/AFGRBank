using AFGRBank.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Main
{
    public class BankingMain
    {
        public void MainMenu()
        {
            string logo =
                "ASCII Placeholder\n" +
                "ASCII Placeholder\n" +
                "ASCII Placeholder\n";

            string[] mainMenuOptions = { "Login", "Exit" };
            bool isContinue = true;
            while (isContinue)
            {
                MainMenuOptions selectedOption = Menu.ReadOption<string, MainMenuOptions>(logo, mainMenuOptions);
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

            //Login.Login(username, password);
        }

        public void UserMenu()
        { 
        
        }

        public void AdminMenu()
        {

        }

        public void TransferMenu()
        {

        }

        public void LoanMenu()
        {
        }   

        public void AccountMenu()
        {

        }

    }
}
