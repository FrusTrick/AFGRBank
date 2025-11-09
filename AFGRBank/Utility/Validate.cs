using AFGRBank.Exchange;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AFGRBank.Utility
{
    /// <summary>
    /// Class contains custom-made methods for error handling.
    /// To be used for validating inputs and converting one data type to another.
    /// </summary>
    public class Validate
    {
        // =======================================================================================================================
        // Methods for custom error handling to be used to validate and convert user inputs.
        // Most methods are built-in with wrapped while-statements that continuously loops until the user inputs correctly.
        // No hardcoded text, each method are called with 2 or more text parameters that will be used to display text to the user.
        // The first string parameter should always prompt user to input something.
        // The other string parameters should display error messages which informs that user did not input correctly and that they
        // should try again.
        // More info on the methods.
        // =======================================================================================================================


        /// <summary>
        /// Method only returns input from ReadLine() with no validation.
        /// Any validation will have to be performed outside this method.
        /// Use <paramref name="msgPrompt"/> to define a text that will instruct the user to write the desired input.
        /// </summary>
        /// <param name="msgPrompt"> A custom defined text that ideally instructs the user to write the desired input.</param>
        /// <returns>A string.</returns>
        public static string GetInput(string msgPrompt)
        {
            Console.WriteLine(msgPrompt);
            string input = Console.ReadLine().Trim();
            return input;
        }

        // Method will continuously loop until user inputs a string that isn't null, empty, or only whitespace characters, and then returns it.
        // Any type conversion should be done outside of method.
        /// <summary>
        /// Method will continuously loop until user inputs a string that isn't null, empty, or only whitespace characters.
        /// Use <paramref name="msgPrompt"/> to define a text that will instruct the user to write the desired input.
        /// Use <paramref name="msgErrorEmpty"/> to define a text that will be printed out when the user's input is empty.
        /// </summary>
        /// <returns>A non-empty string.</returns>
        /// <param name="msgPrompt"> A custom defined text that ideally instructs the user to write the desired input.</param>
        /// <param name="msgErrorEmpty">A custom defined text that will be printed out whenever the user writes an empty input.</param>
        public static string GetInput (string msgPrompt, string msgErrorEmpty)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string input = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(msgErrorEmpty);
                    continue;
                }
                return input;
            }
        }


        /// <summary>
        /// Prompts user, and continuously loops until user inputs a valid email address syntax.
        /// </summary>
        /// <remarks>
        /// Uses RegEx. Not case-sensitive.
        /// </remarks>
        /// <param name="msgPrompt">Custom defined text that instructs the user to type an email address.</param>
        /// <param name="msgErrorEmail">Custom defined error text that is displayed when user input is not a valid email address.</param>
        /// <returns>A valid email address as string.</returns>
        public static string GetInputEmail(string msgPrompt, string msgErrorEmail)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string input = Console.ReadLine()
                    .Trim()
                    .ToLower();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(msgErrorEmail);
                    continue;
                }
                string strRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
                Regex regex = new Regex(strRegex);
                if (!regex.IsMatch(input))
                {
                    Console.WriteLine(msgErrorEmail);
                    continue;
                }
                return input;
            }
        }


        /// <summary>
        /// Prompts user, and continuously loops until user inputs a valid phone number syntax.
        /// </summary>
        /// <remarks>
        /// Uses RegEx. Numbers only.
        /// </remarks>
        /// <param name="msgPrompt">Custom defined text that instructs the user to type an email address.</param>
        /// <param name="msgErrorPhone">Custom defined error text that is displayed when user input is not a valid phone number.</param>
        /// <returns>A valid phone number as string.</returns>
        // Class Regex Represents an immutable regular expression.
        // Phone Format         Regex Pattern
        // xxxxxxxxxx           ^[0 - 9]{ 10}$
        // +xx xx xxxxxxxx      ^\+[0 - 9]{ 2}\s +[0 - 9]{ 2}\s +[0 - 9]{ 8}$
        // xxx - xxxx - xxxx    ^[0 - 9]{ 3} -[0 - 9]{ 4}-[0 - 9]{ 4}$
        public static string GetInputPhone(string msgPrompt, string msgErrorPhone)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string input = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(msgErrorPhone);
                    continue;
                }
                string strRegex = @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";
                Regex regex = new Regex(strRegex);
                if (!regex.IsMatch(input))
                {
                    Console.WriteLine(msgErrorPhone);
                    continue;
                }
                return input;
            }
        }


        public static bool ToInt(string toParse, string msgErrorParse)
        {
            bool success = Int32.TryParse(toParse, out int parsedValue);
            if (!success)
            {
                Console.WriteLine(msgErrorParse);
                return false;
            }
            return true;
        }

        public static int StringToInt(string msgPrompt, string msgErrorEmpty, string msgErrorParse)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string stringToParse = Console.ReadLine()
                    .Trim();
                if (string.IsNullOrWhiteSpace(stringToParse))
                {
                    Console.WriteLine(msgErrorEmpty);
                    continue;
                }
                if (!int.TryParse(stringToParse, out int parsedValue))
                {
                    Console.WriteLine(msgErrorParse);
                    continue;
                }
                return parsedValue;
            }
        }

        public static int StringToInt(string msgPrompt, string msgErrorEmpty, string msgErrorParse, string msgErrorOutOfRange)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string stringToParse = Console.ReadLine()
                    .Replace(",", string.Empty)
                    .Trim();
                if (string.IsNullOrWhiteSpace(stringToParse))
                {
                    Console.WriteLine(msgErrorEmpty);
                    continue;
                }
                int parsedValue;
                try
                {
                    parsedValue = int.Parse(stringToParse);
                }
                catch (FormatException)
                {
                    Console.WriteLine(msgErrorParse);
                    continue;
                }
                catch (OverflowException)
                {
                    Console.WriteLine(msgErrorOutOfRange);
                    continue;
                }
                return parsedValue;
            }
        }


        public static decimal StringToDecimal(string msgPrompt, string msgErrorEmpty, string msgErrorParse)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string stringToParse = Console.ReadLine()
                    .Trim();
                if (string.IsNullOrWhiteSpace(stringToParse))
                {
                    Console.WriteLine(msgErrorEmpty);
                    continue;
                }
                if (!decimal.TryParse(stringToParse, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal parsedValue))
                {
                    Console.WriteLine(msgErrorParse);
                    continue;
                }
                return parsedValue;
            }
        }

        public static CurrencyExchange.CurrencyNames StringToCurrencyName(string msgPrompt, string msgErrorEmpty, string msgErrorParse)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string? stringToParse = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(stringToParse))
                {
                    Console.WriteLine(msgErrorEmpty);
                    continue;
                }
                if (!Enum.TryParse(stringToParse, true, out CurrencyExchange.CurrencyNames parsedValue))
                {
                    Console.WriteLine(msgErrorParse);
                    continue;
                }
                return parsedValue;
            }
        }

        public static Guid StringToGuid(string msgPrompt, string msgErrorEmpty, string msgErrorParse)
        {
            while (true)
            {
                Console.WriteLine(msgPrompt);
                string? stringToParse = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(stringToParse))
                {
                    Console.WriteLine(msgErrorEmpty);
                    continue;
                }
                if (!Guid.TryParse(stringToParse, out Guid parsedValue))
                {
                    Console.WriteLine(msgErrorParse);
                    continue;
                }
                return parsedValue;
            }
        }

    }
}
