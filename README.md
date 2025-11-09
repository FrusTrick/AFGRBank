# AFGRBank - Admin Class

## Overview
The `Admin` class is located in the `AFGRBank.UserType` namespace.  
It inherits from the `User` class and represents a bank administrator with special privileges.  
Admins can manage user accounts, update exchange rates, create loans, and confirm pending transactions.

---

## Class Information
- **Class Name:** `Admin`  
- **Inherits from:** `User`  
- **Namespace:** `AFGRBank.UserType`

---

## Properties

- `IsAdmin` (`bool`): Indicates that this user is an admin. Defaults to `true`.

---

## Methods

### `AddFunds(User user, Account account, decimal amount)`
Adds funds to a user's account after validating inputs.  
- Checks that `user` and `account` exist.  
- Ensures `amount` is greater than zero.  
- Updates the account balance.

### `RemoveFunds(User user, Account account, decimal amount)`
Removes funds from a user's account if sufficient balance exists.  
- Validates `user` and `account`.  
- Ensures `amount` is greater than zero.  
- Deducts the amount if funds are sufficient.

### `CreateUser(string username, string password, string name, string surName, string email, int phoneNumber, string address, List<User> userList)`
Creates a new user and adds it to the list.  
- Checks for unique username.  
- Returns the updated user list including the new user.

### `UpdateCurrencyRates(CurrencyNames currencyName, decimal updatedAmount)`
Updates the `CurrencyRates.json` file with a new exchange rate.  
- Ensures the JSON file exists.  
- Updates or adds the currency rate.  
- Uses JSON serialization with enums.

### `CreateLoan(User user, Account account, decimal loanAmount, CurrencyNames currency, decimal interestRate)`
Creates a loan for a user and credits it to the account.  
- Checks eligibility based on total funds and existing loans.  
- Calculates monthly payment and repayment period.  
- Adds the loan to `User.LoanList` and updates the account balance.

### `ViewPendingTransactions(List<Transaction> pending)`
Displays pending transactions for review.  
- Removes expired transactions.  
- Shows a menu of pending transactions.  
- Admin can confirm or decline each transaction.

---

## Dependencies
The `Admin` class interacts with:

- `AFGRBank.BankAccounts` – Uses `Account` to manage balances.  
- `AFGRBank.Exchange` – Uses `CurrencyExchange` and `CurrencyNames` for exchange rates.  
- `AFGRBank.Loans` – Uses `Loan` to create and manage loans.  
- `AFGRBank.Main` – Accesses `Login` for the user list.  
- `AFGRBank.Utility` – Uses `Menu` and `Transaction` for UI and transaction management.

---

## Relationships
- **Inheritance:** `Admin` → `User`  
- **Associations:** Uses `Account`, `Loan`, `Transaction`, `Menu`, `CurrencyExchange`

---

## Notes
- Admin actions include creating users, managing funds, updating exchange rates, creating loans, and confirming transactions.  
- The `IsAdmin` property is always `true`.  
- Loan and transaction methods include automated calculations and safety checks.
