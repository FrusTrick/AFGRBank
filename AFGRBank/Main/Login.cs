using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.UserType;

namespace AFGRBank.Main
{
    public class Login
    {
        // Every user and admin accounts are stored inside the "UserList" property.

        // If "LoggedInUser" is null, that means user is not logged in. 
        // The current signed in user info is stored inside LoggedInUser. Used Login.LoggedInUser.
        // DO NOT use the Login class to check for current user info.

        public List<User> UserList { get; set; } = new List<User>();
        public User? LoggedInUser { get; set; }


        public void LoginUser(string username, string password)
        {
            try
            {
                LoggedInUser = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed outright: {ex.Message}");
                Console.WriteLine($"Press any key to continue...");
                Console.ReadKey();
            }
            return;

        }

        public void LogoutUser()
        {
            if (LoggedInUser != null)
            {
                Console.WriteLine($"{LoggedInUser.UserName} has logged out.");
                Console.ReadKey();
                LoggedInUser = null;
            }
            else
            {
                Console.WriteLine($"No one is logged in.");
            }
        }
    }
}
