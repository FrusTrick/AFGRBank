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
- **Line 35 - line 43:** Instanced objects of the following classes: `User`, `Admin`, `Login`, `CurrencyExchange`, `CheckingsAccount`, `SavingsAccount`, `Transaction`, `PendingTransaction`, `Loan`. This is so that all menus may access every class' methods.
- `pendingTransaction` (`List<Transaction>`): A "waiting list" where all transactions are stored inside before they can be confirmed, either by the `_timer`, or by an admin (that you presumably bribed <_<)

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

### Properties
- `IsAdmin` (`bool`): Indicates admin status; always `true`.  

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
- `IsAdmin` is always `true`.  
- Methods include checks and calculations for safe operation.

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
