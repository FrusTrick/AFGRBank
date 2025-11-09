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
