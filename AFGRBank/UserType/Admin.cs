using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFGRBank.Main;

namespace AFGRBank.UserType
{
    public class Admin : User
    {
        public bool IsAdmin { get; set; }

        // Calls to create a new user
        // Loops through to make sure there are no duplicate UserNames
        // If there aren't, returns a new userlist with the created user
        public List<User> CreateUser(string username, string password, string name, string surName, int phoneNumber, string address, List<User> userList)
        {
            List<User> newUserList = userList;

            foreach (User user in userList)
            {
                if (username != user.UserName)
                {
                    User newUser = new User();
                    newUser.UserName = username;
                    newUser.Password = password;
                    newUser.Name = name;
                    newUser.PhoneNumber = phoneNumber;
                    newUser.Address = address;
                    newUserList.Add(newUser);
                    Console.WriteLine($"{UserName} successfully created.");
                }
                else
                {
                    Console.WriteLine("The username is already taken.");
                }
            }
            
            return newUserList;
        }

        public void UpdateCurrencyRates()
        {
            
        }
    }
}
