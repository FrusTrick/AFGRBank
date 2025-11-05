using AFGRBank.BankAccounts;
using AFGRBank.Exchange;
using AFGRBank.Loans;
using AFGRBank.Utility;
using AFGRBank.UserType;
using static AFGRBank.Exchange.CurrencyExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AFGRBank.Main
{
    public partial class BankingMain
    {

        #region "AdminMenu() methods"
        private void CreateUserMenu()
        {
            string username = string.Empty;
            string password = string.Empty;
            string name = string.Empty;
            string surname = string.Empty;
            string email = string.Empty;
            int phoneNumber = 0;
            string address = string.Empty;


            bool isContinue = true;
            while (isContinue)
            {
                string text = $"User account creation:";
                string[] createUserMenuOptions = {
                    $"Edit username              Current: {username}",
                    $"Edit password              Current: {password}",
                    $"Edit name                  Current: {name}",
                    $"Edit surname               Current: {surname}",
                    $"Edit email address         Current: {email}",
                    $"Edit phone number          Current: {phoneNumber}",
                    $"Edit address               Current: {address}",
                    $"Create new user",
                    $"Exit"
                };

                CreateUserMenuOptions selectedOption = Menu.ReadOption<string, CreateUserMenuOptions>(text, createUserMenuOptions);
                switch (selectedOption)
                {
                    case CreateUserMenuOptions.EditUsername:
                        Console.Clear();
                        username = Validate.GetInput("Input new username:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditPassword:
                        Console.Clear();
                        password = Validate.GetInput("Input new password:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditName:
                        Console.Clear();
                        name = Validate.GetInput("Input new name:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditSurname:
                        Console.Clear();
                        surname = Validate.GetInput("Input new surname:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditEmail:
                        Console.Clear();
                        email = Validate.GetInput("Input new email address:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.EditPhoneNumber:
                        Console.Clear();
                        phoneNumber = Validate.StringToInt("Input new phone number (numbers only):",
                            "Input cannot be empty. Try again.", "Input was not a number. Try again.",
                            "Input was either too big or too small. Try again."
                            );
                        break;
                    case CreateUserMenuOptions.EditAddress:
                        Console.Clear();
                        address = Validate.GetInput("Input new physical address:", "Input cannot be empty. Try again.");
                        break;
                    case CreateUserMenuOptions.CreateUser:
                        if (username == string.Empty ||
                            password == string.Empty ||
                            name == string.Empty ||
                            surname == string.Empty ||
                            email == string.Empty ||
                            phoneNumber <= 0 ||
                            address == string.Empty)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Error. One or more fields are empty." +
                                $"\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        login.UserList = admin.CreateUser(username, password, name, surname, email, phoneNumber, address, login.UserList);
                        break;
                    case CreateUserMenuOptions.Exit:
                        return;
                }
            }
        }

        private void UpdateCurrencyRatesMenu()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
            string jsonString = File.ReadAllText(jsonPath);

            // The default selected currency and values (TryParse doesn't allow CurrencyNames selectedCurrency = null)
            CurrencyNames selectedCurrency = CurrencyNames.SEK;
            decimal newRates = 1m;

            // These variables will be used to display the selected currency and the new exchange rates in the menu
            string displaySelectedCurrency = string.Empty;
            string displayNewRates = string.Empty;

            while (true)
            {
                // Display .JSON content to string, replacing the curly brackets with whitespaces
                string text = $"Select which currency's exchange rate to update:" +
                    $"\n{jsonString}"
                    .Replace('{', ' ')
                    .Replace('}', ' ');

                // Call ReadOptionIndex with parameters declared inside instead of declaring them outside the method first
                // This is used to update {displaySelectedCurrency} and {displayNewRates} in real time.
                // If you hover over ReadOptionIndex, you can see it takes in two parameters:
                //      questionTetxt is the text displayed above the menu buttons
                //      menuOptions contain the buttons, separated with the comma character ','
                // ReadOptionIndex returns an integer value which is used to compare the switch case below.
                int selectUpdateRateOptions = Menu.ReadOptionIndex(
                    $"{text}",
                    [
                    $"Select currency:        {displaySelectedCurrency}",
                    $"Set new exchange rate:  {displayNewRates}",
                    $"Update",
                    $"Exit",
                    ]);

                switch (selectUpdateRateOptions)
                {
                    case 0: 
                        Console.Clear();
                        // Admin picks which currency to update. There's validation to ensure input matches the correct enum.
                        selectedCurrency = Validate.StringToCurrencyName(
                            $"{text}" +
                            $"Select the currency which needs to update its exchange rate:",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again." 
                            );

                        displaySelectedCurrency = selectedCurrency.ToString();
                        break;
                    case 1:
                        Console.Clear();
                        // Admin inputs the updated exchange rate. There's validation to ensure input is decimal.
                        newRates = Validate.StringToDecimal(
                            $"{text}" +
                            $"Input the updated exchange rate:",
                            $"Input cannot be empty. Try again.",
                            $"Input failed to parse. Make sure the input doesn't contain invalid characters and try again."
                            );

                        displayNewRates = newRates.ToString();
                        break;
                    case 2:
                        // Calls UpdateCurrencyRates which updates the selected currency with the new rates.
                        // Then read .JSON file and update displayText.
                        admin.UpdateCurrencyRates(selectedCurrency, newRates);
                        jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exchange", "CurrencyRates.json");
                        jsonString = File.ReadAllText(jsonPath);
                        break;
                    case 3:
                        return;
                }
            }
        }

        #endregion


    }
}
