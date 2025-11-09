using AFGRBank.UserType;
using AFGRBank.Utility;
using AFGRBank.Main;

namespace AFGRBank
{
    internal class Program
    {
        private static Timer _timer;

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

            //Starts a backgorund timer that checks pending transactions every minute and executes any transactions older than 15 minutes.
            _timer = new Timer(BankingMain.RunTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            
            while (true)
            {
                // GetLoggedInUser() checks if user is already logged in
                // As there's no way to stay logged in after closing program, this will return false
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
