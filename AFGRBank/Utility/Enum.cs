using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Utility
{
    // File to store all enums used in the project
    public enum MainMenuOptions
    {
        Login,
        Exit
    }
    public enum LoginMenuOptions
    {
        Username,
        Password,
        Exit
    }

    public enum UserMenuOptions
    {
        Borrow,
        SetCurrency,
        ViewAccounts,
        ViewInterests,
        ViewTransactions,
        Logout
    }
    public enum AdminMenuOptions
    {
        CreateUser,
        UpdateCurrencyRate,
        Borrow,
        SetCurrency,
        ViewAccounts,
        ViewInterests,
        ViewTransactions,
        Logout
    }
    public enum CreateUserMenuOptions
    {
        EditUsername,
        EditPassword,
        EditName,
        EditSurname,
        EditEmail,
        EditPhoneNumber,
        EditAddress,
        CreateUser,
        Exit
    }
    public enum AccountMenuOptions
    {
        ViewAccountInfo,
        ViewAccountTransactions,
        TransferFunds,
        CreateAccount,
        DeleteAccount,
        Exit,
    }
    public enum SavingsAccountMenuOptions
    {
        ViewAccountInfo,
        ViewAccountTransactions,
        ViewSavingsForecast,
        TransferFunds,
        CreateAccount,
        DeleteAccount,
        Exit,
    }
    public enum TransferMenuOptions
    {
        SetSenderID,
        SetReceiverID,
        SetAmount,
        Exit,
    }
    public enum LoanMenuOptions
    {
        CreateLoan,
        GetLoan,
        EditLoan,
        Exit,
    }

    public enum CurrencyOptions
    { 
        SEK, 
        DKK, 
        EUR, 
        USD, 
        YEN,
        Exit
    }
}
