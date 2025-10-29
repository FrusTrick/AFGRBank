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
        //Initialize the list so it i never null
        public List<User> UserList { get; set; } = new List<User>();
        public User? LoggedInUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }


        //Attemots to log in a user with the provided credentials.
        //If successful, LoggedInUser is set and IsAdmin is uppdated.
        public void LoginUser(string username, string password)
        {

            var user = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                LoggedInUser = user;
                //If User has IsAdmin propertym copy it
                //assumes User class contains bool IsAdmin

                Console.WriteLine($"\nInloggad som: {user.UserName}");
            }

            else
            {
                Console.WriteLine("\nFel användarnamn eller lösenord.");
            }
        }


        //Logs out the current user if any is loggd in.
        public void LogoutUser()
        {
            
            if(LoggedInUser != null)
            {
                Console.WriteLine($"\n{LoggedInUser.UserName} har loggats ut.");
                LoggedInUser = null;
                IsAdmin = false;
            }
            else
            {
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
