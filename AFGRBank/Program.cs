using AFGRBank.Utility;
using AFGRBank.Main;

namespace AFGRBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankingMain menus = new BankingMain();
            Login login = new Login();

            menus.Testing();
            
            
            
            
            
            // If LoggedInUser is null, this means user hasn't logged in yet.
            // I'm assuming LoggedInUser will be used for this.
            while (login.LoggedInUser == null)
            {
                menus.MainMenu();
            }
            if (login.IsAdmin != true)
            { 
                // menus.UserMenu();
            }
            else
            {
                // menus.AdminMenu();
            }

        }
    }
}
