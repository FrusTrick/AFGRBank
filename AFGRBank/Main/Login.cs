using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.UserType;

namespace AFGRBank.Main
{
    public class Login
    {
        // Every user and admin accounts are stored inside the "UserList" property.
        //Initialize the list so it i never null
        public List<User> UserList { get; set; } = new List<User>();
        public User? LoggedInUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        // If "LoggedInUser" is null, that means user is not logged in. 
        // The current signed in user info is stored inside LoggedInUser. Used Login.LoggedInUser.
        // DO NOT use the Login class to check for current user info.

        public List<User> UserList { get; set; } = new List<User>();
        public User? LoggedInUser { get; set; }


        public void LoginUser(string username, string password)
        {
            try
            var user = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                LoggedInUser = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
                LoggedInUser = user;
                //If User has IsAdmin propertym copy it
                //assumes User class contains bool IsAdmin

                Console.WriteLine($"\nInloggad som: {user.UserName}");
            }
            catch (Exception ex)

            else
            {
                Console.WriteLine($"Failed outright: {ex.Message}");
                Console.WriteLine($"Press any key to continue...");
                Console.ReadKey();
            }
            return;

        }


        //Logs out the current user if any is loggd in.
        public void LogoutUser()
        {
            
            if(LoggedInUser != null)
            {
                Console.WriteLine($"{LoggedInUser.UserName} has logged out.");
                Console.ReadKey();
                LoggedInUser = null;
                IsAdmin = false;
            }
            else
            {
                Console.WriteLine($"No one is logged in.");
            }
                Console.WriteLine("\nIngen användare är inloggad.");
            }

        }

        //Returns true if the current session belongs to an admin user.
        public bool ChaeckIsAdmin()
        {
            return IsAdmin;
        }
    }
}
