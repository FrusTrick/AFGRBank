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
            // BankingMain menus will be used to call every menu which can access the other classes
            // PendingTransaction pTransaction will be used to run a timer for approving transactions every 15 minutes
            // PopulateList() data seeds a list with users so we can log in as one of those users
            short loginAttempts = 3;
            BankingMain menus = new BankingMain();
            PendingTransaction pTransaction = new PendingTransaction();
            menus.PopulateList();
            
            

            while (true)
            {
                bool isLoggedIn = menus.GetLoggedInUser();
                while (isLoggedIn == false)
                {
                    menus.MainMenu(loginAttempts);
                    isLoggedIn = menus.GetLoggedInUser();
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
                    isLoggedIn = menus.GetLoggedInUser();
                }
            }

            //menus.Testing(loginAttempts);

        }
    }
}
