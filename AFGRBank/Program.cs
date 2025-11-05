using AFGRBank.UserType;
using AFGRBank.Utility;
using AFGRBank.Main;

namespace AFGRBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // loginAttempts is used for checking how many tries user has to login before being automatically locked out 
            short loginAttempts = 3;
            
            BankingMain menus = new BankingMain();
            
            menus.Testing(loginAttempts);


            menus.MainMenu(loginAttempts);

            
            
            
            
            //// If LoggedInUser is null, this means user hasn't logged in yet.
            //// I'm assuming LoggedInUser will be used for this.
            //while (login.LoggedInUser == null)
            //{
            //    menus.MainMenu();
            //}
            //if (login.IsAdmin != true)
            //{ 
            //    // menus.UserMenu();
            //}
            //else
            //{
            //    // menus.AdminMenu();
            //}

        }
    }
}
