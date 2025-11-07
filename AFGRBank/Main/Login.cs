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
        public static List<User> UserList { get; set; } = new List<User>();
        public User? LoggedInUser { get; set; }

        // Attempts to log in a user by checking the provided username and password.
        /// <summary>
        /// Logs in a user that using the provided username and password.
        /// </summary>
        /// <remarks>
        /// If a matching user is found in the user list, that user is set as the currently logged-in user.  
        /// If no match is found, the LoggedInUser remains null.  
        /// Any unexpected errors (such as a null user list) are caught and handled with a message.
        /// </remarks>
        /// <param name="username">The username to be verified.</param>
        /// <param name="password">The password to be verified.</param>
        /// <return>
        /// This method does not return a value.
        /// </return>
        public void LoginUser(string username, string password)
        {
            try
            {
                // Searches the list of users for a match (same username and password).
                // If found, LoggedInUser will be set to that user, otherwise it will be null.
                LoggedInUser = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors (e.g., if UserList is null).
                Console.WriteLine($"Failed outright: {ex.Message}");
                Console.WriteLine($"Press any key to continue...");
                Console.ReadKey();
            }
            return;

        }

        //Logs out the current user if someone is logged in.
        /// <summary>
        /// Logs out the currently logged-in user if one exists.
        /// </summary>
        /// <remarks>
        /// If a user is logged in, the user will be logged out and the logout message is shown.
        /// If no user is logged in, a message indicating this will be shown.
        /// </remarks>
        public void LogoutUser()
        {
            // Checks if a user is currently logged in
            if (LoggedInUser != null)
            {
                // Announces which user is being logged out
                Console.WriteLine($"{LoggedInUser.UserName} has logged out.");
                Console.ReadKey();
                LoggedInUser = null;
            }
            else
            {
                // Message shown if no user is logged in
                Console.WriteLine($"No one is logged in.");
            }
        }
    }
}
