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
            
            // BankingMain menus will be used to call every menu which can access the other classes.
            BankingMain menus = new BankingMain();
            
            menus.PopulateList();
            
            

            while (true)
            {
                bool isLoggedIn = menus.GetUserIsLoggedIn();
                while (isLoggedIn == false)
                {
                    menus.MainMenu(loginAttempts);
                    isLoggedIn = menus.GetUserIsLoggedIn();
                } 
                while (isLoggedIn == true)
                {
                    bool isAdmin = menus.GetIsAdmin();
                    if (isAdmin == true)
                    {
                        menus.AdminMenu();
                    }
                    else
                    {
                        menus.UserMenu();
                    }
                    isLoggedIn = menus.GetUserIsLoggedIn();
                }
            }

            //menus.Testing(loginAttempts);

        }
    }
}
