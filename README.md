# AFGRBank Classes Documentation

---

## AFGRBank - Program Class
### Overview
The program entry point. Contains as little clutter as we could.
The only declared items are `Timer` `_timer`, which is used to finalize `Transaction` objects after 15 minutes; and `short` `loginAttempts`, used to determine how many failed sign in attempts an user has before they are kicked out of program.

---

## AFGRBank - BankingMain Class
### Overview
The `BankingMain` is located in the `AFGRBank.Main` namespace.
Is "first" class encountered by user and contains most of the menus and submenus used to navigate the applications. 
Due it being instanced immediately upon program execution, `BankingMain` also includes some miscellaneous methods, unrelated to menus but are required for other classes to function, as well as some methods that was had yet to be placed in their respective classes.
> There is nothing more permanent than a temporary solution

### Field
- **Line 35 - line 43:** Instanced objects of the following classes: `User`, `Admin`, `Login`, `CurrencyExchange`, `CheckingsAccount`, `SavingsAccount`, `Transaction`, `PendingTransaction`, `Loan`. This is so that the menus may access every class' methods.
- `pendingTransaction` (`List<Transaction>`): A "waiting list" where all transactions await to be confirmed, either by the `_timer`, or by an admin.

### Methods
- `ListUserAccountsMenu(User user)` - List out every bank account that belongs to the `user` parameter, and allows the user to select and return one bank account. Depending on implementation, `user` can be the currently logged in user or a different user entirely.
- `MainMenu(loginAttempts)` - Initial screen, user may choose to login or quit program.
- `LoginMenu(loginAttempts)` - Allows user to input username and password to try and login. After `loginAttempts` unsuccessful login tries, immediately quits program.

- `UserMenu()` - If `login.LoggedInUser` is not an `Admin` class, run this. This menu allows user to transfer money between bank accounts, to view all their accounts, to create new bank accounts, to view user's transaction history made by or to user, to view users loan history, and to logout.
- `TransferMenu(User senderUser)` - Submenu that allows user to send money from one of their accounts to either another of their account, or to a different user's account. The transfer money is also recalculated to be the same across different currency types.
- `ViewSelectedAccountMenu(Account selectedAccount, List<Account> accountList)` - Allows the user to view the bank account information, to view the bank account's transaction history, to change the bank account's currency type, to view saving forecast, and to delete the account entirely. Uses methods stored in the `Account` class and its subclasses.
- `CreateNewAccountMenu(User user)` - Opens a menu that allows user to create a new bank account. User may change currency and account type. 
- `SetBankAccountType()` - Sets the bank account type. Used in `CreateNewAccountMenu()`.

- `AdminMenu()` - If `login.LoggedInUser` is an `Admin` class, run this. This menu allows admin user to access user accounts to deposit, withdraw or create loan to, to create a new `User` class (i.e a new user), to manually update a currency's exchange rate, to view all waiting `Transaction` objects in `pendingTransaction`, and to logout.
- `CreateUserMenu()` - Opens submenu that allows admin to create a new user. Admin can input user information and sends that to `CreateUser()` in `Admin` class.
- `UpdateCurrencyRatesMenu()` - Opens submenu for the admin to manually update a currency exchange rate.
- `AdminViewUserAccountMenu(User selectedUser, Account selectedAccount)` - Opens a submenu similar to `ViewSelectedAccountMenu()` except it contains admin specific options. The admin may deposit or withdraw money to and from, or create a loan for `selectedAccount`.
- `CreateLoanMenu(User loanTaker, Account loanTakerAccount)` - Creates a loan for a user and deposit it to their account. Admin inputs the loan amount and interest rate.

- `GetIsAdmin()` - Checks if the currently logged in user is an `Admin` class or not.
- `GetLoggedInUser()` - Checks if there's a logged in user. Used during the the program startup.
- `GetJSONCurrencyRatesToString()` - Returns the currency type and their respective exchange rates from a JSON file as a string in row format.
- `PopulateList()` - Data seeds the program with one user and one admin.
- `RunTast(object state)` and `TransactionCheck` - Confirms transaction in the `pendingTransaction` "waiting list" after 15 minutes.

### Dependencies 
- `AFGRBank.BankAccounts`
- `AFGRBank.Exchange`
- `AFGRBank.Loans`
- `AFGRBank.UserType`
- `AFGRBank.Utility`
- `AFGRBank.Exchange.CurrencyExchange` - uses `CurrencyExchange` as a `static` 

### Relationships
- **Associations:** `User`, `Admin`, `Login`, `CurrencyExchange`, `CheckingsAccount`, `SavingsAccount`, `Transaction`, `PendingTransaction`, `Loan`.

---

## AFGRBank - User Class

### Overview
The `User` class is located in the `AFGRBank.UserType` namespace.  
Represents a standard bank customer who can hold accounts, make transactions, and take out loans.

### Class Information
- **Class Name:** `User`  
- **Namespace:** `AFGRBank.UserType`  
- **Base Class:** `object`  

### Properties
- `UserName` (`string`): Username for login.  
- `Password` (`string`): Password for login.  
- `Name` (`string`): User's first name.  
- `Surname` (`string`): User's last name.  
- `Email` (`string`): User's email address.  
- `Address` (`string`): Physical address.  
- `PhoneNumber` (`int`): Contact number.  
- `Accounts` (`List<Account>`): List of the user's bank accounts.  
- `TransactionList` (`List<Transaction>`): All transactions associated with the user.  
- `LoanList` (`List<Loan>`): Active loans of the user.  
- `TotalFunds` (`decimal`, private): Cached total funds across all accounts.  

### Methods
- `SetCurrency(Account account, CurrencyExchange.CurrencyNames currency)` – Sets the currency type of an account.  
- `ViewAccounts() : List<Account>` – Returns all accounts owned by the user.  
- `CalculateAccountInterest(SavingsAccount account, decimal interestRate) : decimal` – Calculates interest for a savings account.  
- `AddLoan(Loan loan)` – Adds a loan to the user's loan list.  
- `ViewAllTransactions() : List<Transaction>` – Returns all transactions associated with the user's accounts.  
- `GetTotalFunds() : decimal` – Calculates total funds across all user accounts.  

### Dependencies
- `AFGRBank.BankAccounts` – Uses `Account` and `SavingsAccount`.  
- `AFGRBank.Exchange` – Uses `CurrencyExchange.CurrencyNames`.  
- `AFGRBank.Loans` – Uses `Loan`.  
- `AFGRBank.Main` – May interact with main application logic.

### Notes
- Users are standard bank clients with no admin privileges.  
- Total funds and interest calculations are specific to `SavingsAccount`.  
- Designed to allow future extensions, such as currency conversion.

---

## AFGRBank - Admin Class

### Overview
The `Admin` class is located in the `AFGRBank.UserType` namespace.  
Represents a bank administrator with special privileges.  
Admins can manage user accounts, update exchange rates, create loans, and confirm pending transactions.

### Class Information
- **Class Name:** `Admin`  
- **Namespace:** `AFGRBank.UserType`  
- **Inherits From:** `User`  

### Methods
- `AddFunds(User user, Account account, decimal amount)` – Adds funds to a user's account.  
- `RemoveFunds(User user, Account account, decimal amount)` – Removes funds from a user's account.  
- `CreateUser(string username, string password, string name, string surName, string email, int phoneNumber, string address, List<User> userList) : List<User>` – Creates a new user and adds to the user list.  
- `UpdateCurrencyRates(CurrencyNames currencyName, decimal updatedAmount)` – Updates exchange rates.  
- `CreateLoan(User user, Account account, decimal loanAmount, CurrencyNames currency, decimal interestRate)` – Creates a loan for a user.  
- `ViewPendingTransactions(List<Transaction> pending)` – Displays pending transactions for review and approval.

### Dependencies
- `AFGRBank.BankAccounts` – Uses `Account` for balance management.  
- `AFGRBank.Exchange` – Uses `CurrencyExchange` for exchange rates.  
- `AFGRBank.Loans` – Uses `Loan` for loan management.  
- `AFGRBank.Main` – Accesses login and user list.  
- `AFGRBank.Utility` – Uses `Menu` and `Transaction` for UI and transactions.

### Relationships
- **Inheritance:** `Admin` → `User`  
- **Associations:** Uses `Account`, `Loan`, `Transaction`, `Menu`, `CurrencyExchange`.

### Notes
- Admins manage users, funds, loans, and transactions.  
- Methods include checks and calculations for safe operation.

---

## AFGRBank - Account Class

### Overview
The `Account` base class is located in the `AFGRBank.BankAccounts` namespace.  
Represents a bankaccount base class that contains properties tied to bank accounts. 

### Class Information
- **Class Name:** `Account`  
- **Namespace:** `AFGRBank.BankAccounts`
-**Base class for:** `Checkings Account` and `Savings Account`
  
### Methods
- `CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)` - virtual method for account creation.
- `DeleteAccount(List<Account> accountList, Guid accountId)` - Removes an account as long as it has no funds.
- `ViewTransactions(Account account)` - Displays all historical transactions to and from a specific account.
- 
### Dependencies
-`AFGRBank.Utility` uses `Menu` for displaying menu choices regarding confirmation upon account deletion.
-`AFGRBank.Main` uses `Transaction` to access transactions in the ViewTransactions method.
-`AFGRBank.Exchange` uses `CurrencyExchange` to store Currency info in the Currency property.

### Relationships
-**Base for:** `CheckingsAccount` and `SavingsAccount`
-**Associations:** Uses `Transaction`, `Menu`, `CurrencyExchange`.

### Notes
-Base class for all bank account related classes
-Handles creation, read and deleteion functionality for bank accounts

---

## AFGRBank - CheckingsAccount Class

### Overview
The `CheckingsAccount` class is located in the `AFGRBank.BankAccounts` namespace and inherits from `Account` in the same namespace.  
Represents a checkings account that contains an extra property for class confirmation. 

### Class Information
- **Class Name:** `CheckingsAccount`  
- **Namespace:** `AFGRBank.BankAccounts`
-**Inherits from:** `Account`
  
### Methods
- `CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)` - override method for checkings account creation.
- 
### Dependencies
-`AFGRBank.Main` uses `Transaction` to access transactions in the ViewTransactions method.
-`AFGRBank.Exchange` uses `CurrencyExchange` to store Currency info in the Currency property.

### Relationships
-**Inheritance:** `Account`
-**Associations:** Uses `Transaction`, `CurrencyExchange`.

### Notes
- Contains a boolean property for easy identification of account type.

---

## AFGRBank - SavingsAccount Class

### Overview
The `SavingsAccount` class is located in the `AFGRBank.BankAccounts` namespace and inherits from `Account` in the same namespace.  
Represents a savings account that contains an extra property for class confirmation as well as interest rate associated with the account. 

### Class Information
- **Class Name:** `SavingsAccount`  
- **Namespace:** `AFGRBank.BankAccounts`
-**Inherits from:** `Account`
  
### Methods
- `CreateAccount(List<Account> accountList, CurrencyExchange.CurrencyNames currency)` - override method for savings account creation.
- `SavingsForecast(SavingsAccount savingsAcc, int years)` - displays a forecast regarding account funds over a period of time using compounding intrest.

### Dependencies
-`AFGRBank.Main` uses `Transaction` to access transactions in the ViewTransactions method.
-`AFGRBank.Exchange` uses `CurrencyExchange` to store Currency info in the Currency property.

### Relationships
-**Inheritance:** `Account`
-**Associations:** Uses `Transaction`, `CurrencyExchange`.

### Notes
- Contains a boolean property for easy identification of account type.
- Contains interest rate associated with account.

---

## AFGRBank - Transaction Class

### Overview
The `Transaction` class is located in the `AFGRBank.Main` namespace. 
Represents transactions and contains methods handling them.

### Class Information
- **Class Name:** `Transaction`  
- **Namespace:** `AFGRBank.Main`
  
### Methods
- `ConfirmTransaction(Transaction senderTransaction, Transaction recipientTransaction)` - Confirms a transaction by finalizing the transfer between a sender and a recipient..
- `RemoveExpiredTransactions()` - Removes Transaction objects from the PendingTransaction list after 15 minutes has passed.
- `DisplayAllTransactions(User user)` - Displays all transactions to and from all accounts held by a specific user.

### Dependencies
-`AFGRBank.Exchange` uses `CurrencyExchange` to handle automatic currency conversion when funds are transferred between two accounts.
-`AFGRBank.UserType` uses `User` to access user related properties.

### Relationships
-**Associations:** Uses `User`, `CurrencyExchange`, `PendingTransaction`.

### Notes
- Responsible for facilitating transactions themselves, not creating them.
- For creation of transactions, see `PendingTransactions`.

---

## AFGRBank - PendingTransaction Class

### Overview
The `PendingTransaction` class is located in the `AFGRBank.Main` namespace. 
Represents transaction creation and contains methods handling them while they are pending.

### Class Information
- **Class Name:** `PendingTransaction`  
- **Namespace:** `AFGRBank.Main`
  
### Methods
- `PrepFundsTransfer(List<User> userList, Guid senderAccID, Guid recipientAccID, decimal funds)` - Sets up a new transaction between two accounts and adds them to the PendingTransaction list.
- `FinalizeTransaction(Transaction senderTransaction, Transaction recipientTransaction)` - Called upon by `ExecutePendingTransactions` to execute the transfer of funds and transaction history update for both accounts.
- `ExecutePendingTransactions()` - Executes pending transactions that have been delayed for more than 15 minutes.

### Dependencies
-`AFGRBank.Exchange` uses `CurrencyExchange` to handle automatic currency conversion when funds are transferred between two accounts.
-`AFGRBank.UserType` uses `User` to access user related properties.

### Relationships
-**Associations:** Uses `User`, `CurrencyExchange`, `Transaction`.

## AFGRBank - Login Class

### Overview
The `Login` class is located in the `AFGRBank.Main` namespace.  
It manages user authentication by allowing users to sign in and sign out.  
It also holds a shared list of all registered users in the system.

### Class Information
- **Class Name:** `Login`  
- **Namespace:** `AFGRBank.Main`  
- **Base Class:** `object`  

### Properties
- `UserList` (`List<User>`, static): Stores all user and admin accounts in the system.  
- `LoggedInUser` (`User?`): The currently logged-in user. `null` if no user is logged in.

### Methods
- `LoginUser(string username, string password)`  
  Attempts to log in a user by matching the provided username and password against `UserList`.  
  If a match is found, that user becomes the active session user.

- `LogoutUser()`  
  Logs out the currently logged-in user.  
  Displays a message depending on whether a user was logged in or not.

### Dependencies
- `AFGRBank.UserType` – Uses `User` for account information.

### Notes
- Do not use this class to check current user information globally; only to handle login state changes.
- User validation is handled by searching the shared user list.

---

## AFGRBank - Loan Class

### Overview
The `Loan` class is located in the `AFGRBank.Loans` namespace.  
It represents a financial loan that belongs to a user and contains details such as currency, interest rate, duration, and amount.

### Class Information
- **Class Name:** `Loan`  
- **Namespace:** `AFGRBank.Loans`  
- **Base Class:** `object`  

### Properties
- `Currency` (`CurrencyExchange.CurrencyNames`): The currency in which the loan is taken.  
- `InterestRate` (`decimal`): The loan's interest rate.  
- `StartDate` (`DateTime`): When the loan begins.  
- `EndDate` (`DateTime`): When the loan is scheduled to be fully repaid.  
- `LoanAmount` (`decimal`): The borrowed amount.

### Methods
- `CreateLoan(currency, interestRate, amount, months)`  
  Creates and initializes a new loan with specified values.

- `GetLoan()`  
  Displays the loan details in the console.

- `EditLoan(currency, interestRate, amount, months)`  
  Updates the existing loan with new values.

- `CalcMaxLoan(income, multiplier)`  
  Calculates the maximum possible loan a user can receive based on income.

- `DisplayAllLoans(User user)`  
  Displays a list of all loans associated with a specific user.

### Dependencies
- `AFGRBank.Exchange` – Uses `CurrencyExchange.CurrencyNames`.  
- `AFGRBank.UserType` – Uses `User` and accesses `LoanList`.  
- `System` – Used for date and console output.

### Notes
- Designed to allow loan creation, editing, and viewing in an organized structure.
- Works closely with user loan lists for tracking multiple loans.

---

## AFGRBank - CurrencyExchange Class
### Overview
The `BankingMain` is located in the `AFGRBank.Exchange` namespace.
Represents whoever or whatever actually calculates the exchange rates of the world's currencies.

### Enum
- `CurrencyNames`: The shorthand names of currencies (contains five as of now: `SEK`, `DKK`, `EUR`, `USD`, `YEN`). Inside CurrencyRates.JSON,  each currency has a corresponding exchange rate value. The exchange rate are based on their real life exchange rate to SEK (might be out of date).

### Methods
- `CalculateExchangeRate(string senderCurrencyName, string recipientCurrencyName, decimal amount)` - Called during money transfer between bank accounts so that the transfer amount stays equivalent between different currencies (e.g. if sender uses USD and recipient uses SEK, this method ensures that 100 USD doesn't become 100 SEK).

---

## AFGRBank.Utility Namespace
### Overview
Contains the classes which allows menus and user input fields to function.

### Menu class
#### Overview
Contains methods that displays menu buttons and enables user to navigate in console.
Credits to:
- Johannes for letting us borrow his methods.
- calmBranch for making `ReadOptionIndexList()`.

#### Methods
- `ReadOptionIndex<T>(string questionText, T[] menuOptions)` - The label or prompt text is stored in `questionText`, the buttons are stored in `menuOptions`. This method allows user to navigate through `menuOptions` buttons with the arrow keys and select a button with Enter, which then returns the selected button's index `int` value. This `int` will be to compare inside a `switch` to enter its corresponding `case` code block.
- `ReadOptionIndexList<T>(string questionText, T[] menuOptions)` Similar to `ReadOptionIndex()`, except it uses a `List<T>` rather than an array, which enables a dynamically resizable list of menu buttons. Uses an `if`-statement rather than `switch`.
- `ReadOption<T, TEnum>(string questionText, T[] menuOptions) where TEnum : Enum` - **Depricated**. Similar to `ReadOptionIndex()`, except it returns an enum value. 

### Validate class
#### Overview
Contains methods for validating user inputs, type conversions, and handling errors without crashing while also informing this to user so they know what they did wrong.
These "error informations" are `string` variables which developers can define themselves, allowing for some code flexiblility. 
However, one glaring flaw is that users are unable to leave once these methods are called. Only way is to input the correct characters, which can be difficult if the prompt text misleading.

#### Methods
- `GetInput(string msgPrompt)` - Prompts user input and simply returns a trimmed string.
- `GetInput(string msgPrompt, string msgErrorEmpty)` - Prompts user input and returns a trimmed string. If input is `null`, empty, or contains only whitespace characters, display `msgErrorEmpty` on console before restarting the loop.
- `GetInputMasked(string msgPrompt, string msgErrorEmpty)` - Prompts user input and hides their inputs behind an asterisk, and then returns `List<T>` with both the trimmed string and an the hidden string. Used for password inputs.
- `StringToInt(string msgPrompt, string msgErrorEmpty, string msgErrorParse)` - Prompts user input, converts that input to an `int` value. If input was empty or contained invalid `int` characters, display `msgErrorEmpty` or `msgErrorParse` respectively on console before restarting the loop.
- `StringToInt(string msgPrompt, string msgErrorEmpty, string msgErrorParse)` - Same as the above method, except it converts to `decimal` value.
- `StringToCurrencyName(string msgPrompt, string msgErrorEmpty, string msgErrorParse)` - Same as the above method, except it converts to an `CurrencyName` enum.
- `StringToGuid(string msgPrompt, string msgErrorEmpty, string msgErrorParse)` - **Depricated**. Same as the above method, except it converts to a `Guid` value.

### Dependencies
- `AFGRBank.Exchange` – Uses this namespace for `StringToCurrencyName()`. 

---
