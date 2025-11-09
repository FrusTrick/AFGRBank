# AFGRBank Classes Documentation

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



