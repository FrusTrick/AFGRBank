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
        public List<User> UserList { get; set; }
        public User? LoggedInUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }



        public void LoginUser(string username, string password)
        {

            var user = UserList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                LoggedInUser = user;
                Console.WriteLine($"\nInloggad som: {user.UserName}");
            }

            else
            {
                Console.WriteLine("\nFel användarnamn eller lösenord.");
            }
        }

        public void LogoutUser()
        {
            Console.WriteLine($"\n{LoggedInUser.UserName} har loggats ut.");
            LoggedInUser = null;

        }

        private bool ChaeckIsAdmin()
        {
            if (IsAdmin == true)
                return true;
            else
                return false;
        }
    }
}
