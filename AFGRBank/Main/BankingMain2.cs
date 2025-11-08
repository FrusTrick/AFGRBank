using AFGRBank.BankAccounts;
using AFGRBank.Exchange;
using AFGRBank.Loans;
using AFGRBank.UserType;
using AFGRBank.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using static AFGRBank.Exchange.CurrencyExchange;

namespace AFGRBank.Main
{
    public partial class BankingMain
    {


        #region "UserMenu() methods"

        /// <summary>
        ///     Display menu that allows user to pick one of their accounts to send money to another
        /// </summary>
        /// <param name="senderUser">The currently logged in user</param>
        private void TransferMenu(User senderUser)
        {
            // displayText will be used for showing user inputted values in the menu 
            //      displayText[0] = senderID
            //      displayText[1] = recipientID
            //      displayText[2] = transferFunds
            string[] displayText = { string.Empty, string.Empty, string.Empty };

            string toSenderID = string.Empty;
            string toRecipientID = string.Empty;
            decimal transferFunds = 0;

            while (true)
            {
                string questionText = $"Transfer funds:" +
                    $"\nSender ID:   {displayText[0]}" +
                    $"\nReceiver ID: {displayText[1]}" +
                    $"\nAmount:      {displayText[2]}";

                string[] transferMenuOptions = {
                    "Choose bank account to send from",
                    "Choose bank account to send to",
                    "Choose how much to transfer",
                    "Exit"
                };

                var selectedOption = Menu.ReadOptionIndex(questionText, transferMenuOptions);

                switch (selectedOption)
                {
                    case 0:
                        // Set one of your bank accounts to send money from
                        Console.Clear();

                        Account? senderAccount = ListUserAccountsMenu(senderUser);
                        if (senderAccount == null)
                        {
                            // If user exit ListUserAccountsMenu without picking an account
                            // Exit this case early
                            continue;
                        }

                        toSenderID = senderAccount.AccountID.ToString();
                        displayText[0] = toSenderID;
                        break;

                    case 1:
                        // Input your bank account ID that will receive the money
                        Console.Clear();

                        while (true)
                        {
                            string toRecipient = Validate.GetInput(
                                $"Input the bank account ID to send to:" + $"\nFormat: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
                                $"Input cannot be empty. Try again."
                                );
                            if (!Guid.TryParse(toRecipient, out Guid tempRecipientID))
                            {
                                Console.WriteLine($"Recipient account ID is in an invalid format.");
                                Console.WriteLine($"Press any key to continue...");
                                Console.ReadKey();
                                continue;
                            }
                            break;
                        }
                        displayText[1] = toRecipientID;
                        break;

                    case 2:
                        // Sets the amount of money that will be transferred.
                        // Transfer amount cannot be less than or equal to 0
                        Console.Clear();
                        transferFunds = Validate.StringToDecimal(
                            $"Input amount to transfer",
                            $"Input cannot be empty. Try again",
                            $"Input contained non-numerical characters. Try again."
                            );
                        if (transferFunds <= 0)
                        {
                            transferFunds = 0;
                        }
                        displayText[2] = transferFunds.ToString();
                        break;

                    case 3:
                        // Before calling a method to create a new transaction does an validation check if any
                        // inputs were empty
                        if (toSenderID == string.Empty || toRecipientID == string.Empty || transferFunds == 0)
                        {
                            Console.Clear();

                            if (toSenderID == string.Empty)
                            {
                                Console.WriteLine($"Invalid transfer. Sender ID cannot be empty.");
                            }
                            if (toRecipientID == string.Empty)
                            {
                                Console.WriteLine($"Invalid transfer. Recipient ID cannot be empty.");
                            }
                            if (transferFunds == 0)
                            {
                                Console.WriteLine($"Invalid transfer. Transfer amount can not be zero.");
                            }
                            Console.WriteLine($"Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }
                        // Then tries parse into Guid
                        if (!Guid.TryParse(toSenderID, out Guid senderID))
                        {
                            Console.WriteLine($"Sender account ID is an invalid format.");
                            Console.WriteLine($"Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }
                        if (!Guid.TryParse(toRecipientID, out Guid recipientID))
                        {
                            Console.WriteLine($"Recipient account ID is an invalid format.");
                            Console.WriteLine($"Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }

                        // When all information is validated, call PrepFundsTransfer() which will add two Transaction objects,
                        //      first object is to be stored in sender transaction history,
                        //      second object is to be stored in recipient transaction history;
                        // into PendingTransaction objects awaiting confirmation by an admin or a 15 minute global timer
                        try
                        {
                            var temp = pTransaction.PrepFundsTransfer(Login.UserList, senderID, recipientID, transferFunds);
                            foreach (var transaction in temp)
                            {
                                pendingTransaction.Add(transaction);
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Transaction failed.");
                            Console.WriteLine($"Press any key to continue...");
                            Console.ReadKey();
                        }

                        Console.WriteLine($"Transaction successfully created.");
                        Console.WriteLine($"Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case 4:
                        return;
                }
            }
        }


        /// <summary>
        ///     Print out of properties of <paramref name="selectedAccount"/>
        /// </summary>
        /// <remarks>
        ///     Should be used after the ListUserAccountsMenu() returns a <paramref name="selectedAccount"/>
        /// </remarks>
        /// <param name="selectedAccount">The account to have its properties printed out</param>
        /// <param name="accountList">List of user accounts that the <paramref name="selectedAccount"/> belongs to</param>
        private void ViewSelectedAccountMenu(Account selectedAccount, List<Account> accountList)
        {
            // Default text to be displayed above menu buttons
            string promptText = $"Error. Could not load account info.";

            string[] ViewSelectedAccountMenuOptions = {
                $"View all transactions",
                $"Edit currency",
                $"Delete account",
                $"Exit"
            };

            while (true)
            {
                // Overrides promptText with appropritate values if selectedAccount has a valid account type
                if (selectedAccount is CheckingsAccount)
                {
                    promptText =
                        $"Account ID:   {selectedAccount.AccountID}\n" +
                        $"Account Type: Checkings account\n" +
                        $"Balance:      {selectedAccount.Funds} {selectedAccount.Currency}";
                }
                else if (selectedAccount is SavingsAccount)
                {
                    promptText =
                        $"Account ID:   {selectedAccount.AccountID}\n" +
                        $"Account Type: Savings account\n" +
                        $"Balance:      {selectedAccount.Funds} {selectedAccount.Currency}";
                }

                var selectedOption = Menu.ReadOptionIndex(promptText, ViewSelectedAccountMenuOptions);

                switch (selectedOption)
                {
                    case 0:
                        // Print out all transactions in selectedAccount
                        Console.Clear();

                        selectedAccount.ViewTransactions(selectedAccount);

                        Console.WriteLine($"Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case 1:
                        // Change the currency of the selectedAccount 
                        Console.Clear();

                        string displayCurrencyRates = GetJSONCurrencyRatesToString();

                        CurrencyNames newCurrency = Validate.StringToCurrencyName(
                            $"Input the new currency:" +
                            $"\n{displayCurrencyRates}",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again."
                        );
                        user.SetCurrency(selectedAccount, newCurrency);
                        break;

                    case 2:
                        // Delete selectedAccount
                        Console.Clear();

                        selectedAccount.DeleteAccount(accountList, selectedAccount.AccountID);

                        Console.WriteLine($"Press any key to continue...");
                        Console.ReadKey();
                        return;
                }
            }
        }


        /// <summary>
        ///     Opens menu to be able to create an new bank account
        /// </summary>
        /// <param name="user">The user in which the new account will be stored</param>
        private void CreateNewAccountMenu(User user)
        {
            CurrencyNames currency = CurrencyNames.SEK;
            short? accountType = null;

            // Display available currencies
            string displayCurrencyRates = GetJSONCurrencyRatesToString();

            string[] displayText = { string.Empty, string.Empty };

            while (true)
            {
                string questionText = $"Create new bank account" +
                    $"\n\tType:     {displayText[0]}" +
                    $"\n\tCurrency: {displayText[1]}";
                string[] createNewAccountMenuOptions = {
                    $"Select account type:",
                    $"Set currency:",
                    "Create new account",
                    "Exit",
                };

                var selectedOption = Menu.ReadOptionIndex(questionText, createNewAccountMenuOptions);

                switch (selectedOption)
                {
                    case 0:
                        // User picks what bank account type the new account will be
                        string? tempAccountType = SetBankAccountType();
                        if (tempAccountType == "Checkings")
                        {
                            accountType = 0;
                            displayText[0] = "Checkings account";
                        }
                        if (tempAccountType == "Savings")
                        {
                            accountType = 1;
                            displayText[0] = "Savings account";
                        }
                        break;

                    case 1:
                        // User inputs the currency bank account will use.
                        // Validation methods that displays all the currency available from the .JSON file

                        Console.Clear();

                        CurrencyNames tempCurrency = Validate.StringToCurrencyName(
                            $"Select which currency your new bank account will use:" +
                            $"\n{displayCurrencyRates}",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again."
                            );
                        break;

                    case 2:
                        // If there are no set account type, throw an error message
                        Console.Clear();
                        if (accountType == null)
                        {
                            Console.WriteLine($"Could not create new bank account. Please set an account type.");
                            continue;
                        }
                        else if (accountType == 0)
                        {
                            user.Accounts = cAccount.CreateAccount(user.Accounts, currency);
                        }
                        else if (accountType == 1)
                        {
                            user.Accounts = sAccount.CreateAccount(user.Accounts, currency);
                        }
                        Console.WriteLine($"Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case 3:
                        return;
                }
            }
        }


        /// <summary>
        /// Opens menu for selecting and returning a bank account type
        /// </summary>
        /// <returns>
        ///     <list type="bullet">
        ///         <item>Returns "Checkings" <see cref="string"/> if user wants the bank account type to be a checkings account</item>
        ///         <item>returns "Savings" <see cref="string"/> if user wants the bank account type to be a savings account</item>
        ///         <item>returns <see langword="null"/> if user exits without choosing</item>
        ///     </list>
        /// </returns>
        private string? SetBankAccountType()
        {
            string questionText = $"Select bank account type:";
            string[] setBankAccountType = {
                $"Checkings Account" + 
                    $"\n\tPlaceholder Text...",
                "Savings currency" +
                    $"\n\tAllows you to check future forecast...",
                "Exit",
            };

            while (true)
            {
                var selectedOption = Menu.ReadOptionIndex(questionText, setBankAccountType);

                switch (selectedOption)
                {
                    // If 0 is selected, create Checkingsaccount
                    case 0:
                        return "Checkings";
                    // If 1 is selected, create SavingsAccount
                    case 1:
                        return "Savings";
                    case 2:
                    // If 2 is selected, return null
                        return null;
                }
            }
        }



        private void BorrowMenu()
        {
            while (true)
            {
                string questionText = "Borrow:";
                string[] borrowMenuOptions = { 
                    $"Create loan request",
                    $"Edit loan request",
                    $"Get loan information",
                    $"Exit"
                };
                var selectedOptions = Menu.ReadOptionIndex(questionText, borrowMenuOptions);
                switch (selectedOptions)
                {
                    case 0:
                        CreateLoanMenu();
                        break;
                    case 1:
                        // EditLoan();
                        break;
                    case 2:
                        break;
                    case 3:
                        return;
                }
            }
        }

        private void GetLoanList()
        {
            
            List<Loan> loanList = login.LoggedInUser.LoanList;
        }


        #endregion





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

            while (true)
            {
                string questionText = $"User account creation:";
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

                var selectedOption = Menu.ReadOptionIndex(questionText, createUserMenuOptions);
                switch (selectedOption)
                {
                    case 0:
                        Console.Clear();
                        username = Validate.GetInput("Input new username:", "Input cannot be empty. Try again.");
                        break;
                    case 1:
                        Console.Clear();
                        password = Validate.GetInput("Input new password:", "Input cannot be empty. Try again.");
                        break;
                    case 2:
                        Console.Clear();
                        name = Validate.GetInput("Input new name:", "Input cannot be empty. Try again.");
                        break;
                    case 3:
                        Console.Clear();
                        surname = Validate.GetInput("Input new surname:", "Input cannot be empty. Try again.");
                        break;
                    case 4:
                        Console.Clear();
                        email = Validate.GetInput("Input new email address:", "Input cannot be empty. Try again.");
                        break;
                    case 5:
                        Console.Clear();
                        phoneNumber = Validate.StringToInt("Input new phone number (numbers only):",
                            "Input cannot be empty. Try again.", "Input was not a number. Try again.",
                            "Input was either too big or too small. Try again."
                            );
                        break;
                    case 6:
                        Console.Clear();
                        address = Validate.GetInput("Input new physical address:", "Input cannot be empty. Try again.");
                        break;
                    case 7:
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
                        Login.UserList = admin.CreateUser(username, password, name, surname, email, phoneNumber, address, Login.UserList);
                        break;
                    case 8:
                        return;
                }
            }
        }

        private void UpdateCurrencyRatesMenu()
        {
            // The default selected currency and values (TryParse doesn't allow CurrencyNames selectedCurrency = null)
            CurrencyNames selectedCurrency = CurrencyNames.SEK;
            decimal newRates = 1m;

            // This string array will be used to display the admin selected currency and the new exchange rates in the menu buttons
            string[] displayNewCurrencyAndRates = { string.Empty, string.Empty };

            // Convert .JSON content to string. This will automatically display the updated values
            string displayJSON = GetJSONCurrencyRatesToString();

            // "promptText" is used to for the text above menu buttons
            // Each "updateCurrencyRatesMenuOptions" element are used for the menu buttons
            string promptText = $"Select which currency's exchange rate to update:" +
                $"\n{displayJSON}";
            string[] updateCurrencyRatesMenuOptions = {
                $"Select currency:        {displayNewCurrencyAndRates[0]}",
                $"Set new exchange rate:  {displayNewCurrencyAndRates[1]}",
                $"Update",
                $"Exit",
            };

            while (true)
            {

                var selectedOption = Menu.ReadOptionIndex(promptText, updateCurrencyRatesMenuOptions);

                switch (selectedOption)
                {
                    case 0: 
                        Console.Clear();
                        // Admin picks which currency to update. There's validation to ensure input matches the correct enum.
                        selectedCurrency = Validate.StringToCurrencyName(
                            $"Select currency:" + 
                                $"\n{displayJSON}",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again." 
                            );

                        displayNewCurrencyAndRates[0] = selectedCurrency.ToString();
                        break;
                    case 1:
                        Console.Clear();
                        // Admin inputs the updated exchange rate. There's validation to ensure input is decimal.
                        newRates = Validate.StringToDecimal(
                            $"Input updated exchange rate:" + 
                                $"\n{displayJSON}",
                            $"Input cannot be empty. Try again.",
                            $"Input failed to parse. Make sure the input doesn't contain invalid characters and try again."
                            );

                        displayNewCurrencyAndRates[1] = newRates.ToString();
                        break;
                    case 2:
                        // Updates the selected currency with the new rates.
                        // Then calls 2nd method to update the text with the new rates.
                        admin.UpdateCurrencyRates(selectedCurrency, newRates);
                        displayJSON = GetJSONCurrencyRatesToString();
                        break;
                    case 3:
                        return;
                }
            }
        }

        private void CreateLoanMenu()
        {
            // These variables will used as user defined parameter in CreateLoan() 
            User? loanTaker = null;
            Account? loanTakerAccount = null;
            CurrencyNames currencyName = CurrencyNames.SEK;
            decimal loanAmount = 0;
            decimal interestRate = 0;

            // This will be used to display the loanTaker, loanTakerAccount, currencyName, loanAmount, and interestRate on the button text
            string[] displayText = { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

            while (true)
            {
                string questionText = $"Create new loan request:";
                string[] createLoanMenuOptions = {
                    $"Username:            {displayText[0]}",
                    $"Bank Account ID:     {displayText[1]}",
                    $"Currency:            {displayText[2]}",
                    $"Loan Amount:         {displayText[3]}",
                    $"Interest rate:       {displayText[4]}",
                    $"Create new loan",
                    $"Exit"
                };

                var selectedOption = Menu.ReadOptionIndex(questionText, createLoanMenuOptions);
                switch (selectedOption)
                {
                    case 0: // Input username and find a matching User with LINQ. If there's no match to be found, break out of this case early
                        Console.Clear();
                        string getUsername = Validate.GetInput($"Input username", $"Input cannot be empty. Try again.");

                        loanTaker = Login.UserList.FirstOrDefault(x => x.UserName == getUsername);
                        if (loanTaker == null)
                        {
                            Console.WriteLine($"Failed to find any user with matching username.");
                            Console.WriteLine($"Press any key to exit...");
                            Console.ReadKey();
                            break;
                        }

                        displayText[0] = $"{loanTaker.UserName} [Name: {loanTaker.Name} Surname: {loanTaker.Surname}]";
                        break;
                    
                    case 1: 
                        // Select an account to lend money to.
                        // If loanTaker has not been set yet, display text and exit this case
                        Console.Clear();

                        if (loanTaker == null)
                        {
                            Console.WriteLine($"User is empty. Please fill it and try again. Press any ket to continue...");
                            Console.ReadKey();
                            continue;
                        }

                        loanTakerAccount = ListUserAccountsMenu(loanTaker);

                        // If user doesn't select an account, exit this case
                        if (loanTakerAccount == null)
                        {
                            continue;
                        }

                        displayText[1] = loanTakerAccount.AccountID.ToString();
                        break;

                    case 2:
                        // Select which currency the loan should be in
                        Console.Clear();

                        // GetJSONCurrencyRatesToString display available currencies to be selected
                        string displayCurrencyRates = GetJSONCurrencyRatesToString();

                        currencyName = Validate.StringToCurrencyName(
                            $"Select currency (default is SEK):" +
                            $"\n{displayCurrencyRates}",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again."
                            );

                        displayText[2] = $"{currencyName.ToString()}";
                        break;

                    case 3: 
                        // Sets the amount of money to be lended
                        Console.Clear();
                        loanAmount = Validate.StringToDecimal(
                            $"Input the selected loan amount",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again."
                            );
                        if (loanAmount <= 0 || loanAmount == null)
                        {
                            Console.WriteLine($"Loan can not be below 0. Press any key to continue...");
                            Console.ReadKey();
                        }
                        displayText[3] = loanAmount.ToString();
                        break;

                    case 4:
                        // Sets the loan interest rate
                        Console.Clear();
                        interestRate = Validate.StringToDecimal(
                            $"Input the selected loan amount",
                            $"Input cannot be empty. Try again.",
                            $"Input did not match any existing currency. Try again."
                            );
                        if (interestRate <= 0 || interestRate == null)
                        {
                            Console.WriteLine($"Loan can not be below 0. Press any key to continue...");
                            Console.ReadKey();
                        }
                        displayText[4] = interestRate.ToString();
                        break;

                    case 5:
                        // Creates a loan for the specified user to that particular bank account
                        if (loanTaker == null || loanTakerAccount == null || currencyName == null || loanAmount <= 0 || interestRate <= 0)
                        {
                            Console.WriteLine($"One or more fields has no value. Please fill them.");
                            break;
                        }
                        admin.CreateLoan(loanTaker, loanTakerAccount, loanAmount, currencyName, interestRate);
                        break;

                    case 6:
                        return;
                }
            }


        }

        #endregion





        #region "List account method"

        /// <summary>
        ///     Print out every <paramref name="user"/> bank accounts in a menu, 
        ///     and allows the selection of a specific bank account
        /// </summary>
        /// <param name="user">
        ///     Either the currently logged in user; or an user returned by a method
        /// </param>
        /// <returns>
        /// <list type="bullet">
        ///     <item>
        ///         <description>Returns the selected bank account</description>
        ///     </item>
        ///     <item>
        ///         <description>Returns <see langword="null"/> if no account is selected</description>
        ///     </item>
        /// </list>
        /// </returns>
        public static Account? ListUserAccountsMenu(User user)
        {
            // promptText will be displayed above menu buttons
            // menuOptions are the menu buttons the user can navigate through
            string promptText = "Your bank accounts" +
                "\nSelect a account to view info:";
            List<string> menuOptions = new List<string>();
            List<Account> menuAccounts = new List<Account>();

            // Build the list of all user accounts
            foreach (var account in user.Accounts)
            {
                // Depending on the bank account type, it will print "Checkings" or "Savings"
                if (account is CheckingsAccount)
                {
                    menuOptions.Add(
                        $"Bank ID:      {account.AccountID}\n" +
                        $"Type:         Checkings account\n" +
                        $"Total funds:  {account.Currency} {account.Funds}\n"
                    );
                }
                if (account is SavingsAccount)
                {
                    menuOptions.Add(
                        $"Bank ID:      {account.AccountID}\n" +
                        $"Type:         Savings account\n" +
                        $"Total funds:  {account.Currency} {account.Funds}\n"
                    );
                }
                menuAccounts.Add(account);
            }
            // Adds the exit button last
            menuOptions.Add("Exit");

            while (true)
            {
                // List out a menu with all bank accounts and allows user to select one
                int selectedIndex = Menu.ReadOptionIndexList(promptText, menuOptions);
                var chosenOption = menuOptions[selectedIndex];

                if (chosenOption == "Exit")
                {
                    return null;
                }

                // Run code block if selectedIndex value is smaller than the 
                // (If it falls outside, there's probably a bug somewhere)
                if (selectedIndex < menuAccounts.Count)
                {
                    // Returns selected account
                    Console.Clear();

                    Account selectedAccount = menuAccounts[selectedIndex];
                    return selectedAccount;
                }
            }
        }

        




        #endregion
    }
}
