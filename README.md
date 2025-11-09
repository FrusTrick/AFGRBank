# AFGRBank - Admin Class

## Overview
The `Admin` class is located in the `AFGRBank.UserType` namespace.  
It inherits from the `User` class and represents a bank administrator with special privileges.  
Admins have the capabilities to manage user accounts, update exchange rates, create loans, and confirm pending transactions.

---

## Class Information
**Class Name:** `Admin`  
**Inherits from:** `User`  
**Namespace:** `AFGRBank.UserType`

---

## Properties
None

---

## Methods

### `AddFunds(User user, Account account, decimal amount)`
Adds funds to a user's account after validating the user, account, and amount.

### `RemoveFunds(User user, Account account, decimal amount)`
Removes funds from a user's account if sufficient balance exists.

### `CreateUser(string username, string password, string name, string surName, string email, int phoneNumber, string address, List<User> userList)`
Creates a new `User` and adds it to the provided list, ensuring the username is unique.

### `UpdateCurrencyRates(CurrencyNames currencyName, decimal updatedAmount)`
Updates the `CurrencyRates.json` file with a new exchange rate for the specified currency.

### `CreateLoan(User user, Account account, decimal loanAmount, CurrencyNames currency, decimal interestRate)`
Creates a new loan for a user, checks eligibility, and calculates repayment details automatically.

### `ViewPendingTransactions(List<Transaction> pending)`
Displays pending transactions for admin review and allows confirmation or rejection.

---

## Dependencies
The `Admin` class interacts with several parts of the AFGRBank system:

- **AFGRBank.BankAccounts**  
  Uses the `Account` class to manage user account balances.

- **AFGRBank.Exchange**  
  Uses the `CurrencyExchange` and `CurrencyNames` classes to read and update currency exchange rates.

- **AFGRBank.Loans**  
  Uses the `Loan` class to create and manage user loans.

- **AFGRBank.Main**  
  Accesses the `Login` class to retrieve the global user list.

- **AFGRBank.Utility**  
  Uses `Menu` for user interface prompts and `Transaction` for managing and confirming pending transactions.

---

## Relationships
- **Inheritance:** `Admin` → `User`  
- **Associations:** Uses `Account`, `Loan`, `Transaction`, `Menu`, and `CurrencyExchange`.
