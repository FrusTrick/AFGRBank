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
        //Initialize the list so it is never null
        public List<User> UserList { get; set; } = new List<User>();
        public User? LoggedInUser { get; set; }
        
        // If "LoggedInUser" is null, that means user is not logged in. 
        // The current signed in user info is stored inside LoggedInUser. Used Login.LoggedInUser.
        // DO NOT use the Login class to check for current user info.
        public void LoginUser(string username, string password)
        {
            try
            {

                LoggedInUser = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            var user = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                LoggedInUser = user;
                Console.WriteLine($"\nInloggad som: {user.UserName}");
                Console.WriteLine($"\nInloggad som: {user.UserName}");
            catch (Exception ex)
            else
                Console.WriteLine("\nWrong user or password.");
                Console.WriteLine("\nFel användarnamn eller lösenord.");

        }
        //Logs out the current user if any is loggd in.

        public void LogoutUser()
            if (LoggedInUser != null)
            if(LoggedInUser != null)
                Console.WriteLine($"\n{LoggedInUser.UserName} has been logged out.");
                Console.WriteLine($"\n{LoggedInUser.UserName} har loggats ut.");
                IsAdmin = false;
            }
            else
                Console.WriteLine($"No one is logged in.");
            }

            }
        }
    }
}
