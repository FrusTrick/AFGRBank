using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFGRBank.Utility
{
    internal class Menu
    {
        /// <summary>
        /// Displays a menu with options for the user to choose from. They can navigate using arrow keys up and down. Clears console before displaying.
        /// </summary>
        /// <typeparam name="T">An array that is going to be displayed as strings</typeparam>
        /// <param name="questionText">A string displayed above the option buttons</param>
        /// <param name="menuOptions">The option buttons that is displayed to the user</param>
        /// <returns>Returns the index chosen by user</returns>
        public static int ReadOptionIndex<T>(string questionText, T[] menuOptions)
        {
            // i is used as an array index that goes through menuOptions array as user navigates
            int i = 0;
            while (true)
            {
                Console.Clear();

                // Write out question and display options, currently selected index in menuOptions will be highlighted
                Console.WriteLine(questionText + "\n");
                for (int j = 0; j < menuOptions.Length; j++)
                {
                    // Enumerate through menuOptions, and set background as black and text as white (which is the default console colors)
                    // If user is hovering over a button, set background as white and text as black, making it look highlighted
                    Console.BackgroundColor = i == j ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = i == j ? ConsoleColor.Black : ConsoleColor.White;
                    Console.WriteLine(menuOptions[j]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                // Returns the first key user presses, and compares it to the switch below
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        // If the key is the DownArrow, and if user is not hovering over the last menuOptions button, go down the menuOptions array
                        // Without if, it will throw an exception
                        if (i < menuOptions.Length - 1)
                        {
                            i++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        // If the key is the UpArrow, and if user is not hovering over the first menuOptions button, go up the menuOptions array
                        // Without if, it will throw an exception
                        if (i > 0)
                        {
                            i--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // If the key is Enter, returns index as int
                        // Index will be used in a switch outside ReadOptionIndex()
                        return i;

                }
            }
        }



        /// <summary>
        /// Displays a menu with options for the user to choose from. They can navigate using arrow keys up and down. Clears console before displaying.
        /// </summary>
        /// <typeparam name="T">An array that is going to be displayed as strings</typeparam>
        /// <typeparam name="TEnum">An enum that represents options in menu, where each index is an option</typeparam>
        /// <param name="questionText">A string displayed above the option buttons</param>
        /// <param name="menuOptions">The option buttons that is displayed to the user</param>
        /// <returns>Returns an Enum with chosen enum index</returns>
        public static TEnum ReadOption<T, TEnum>(string questionText, T[] menuOptions) where TEnum : Enum
        {
            // i is used as an array index that goes through menuOptions array as user navigates
            int i = 0;
            while (true)
            {
                Console.Clear();

                // Write out question and display options, currently selected index in menuOptions will be highlighted
                Console.WriteLine(questionText + "\n");
                for (int j = 0; j < menuOptions.Length; j++)
                {
                    // Enumerate through menuOptions, and set background as black and text as white (which is the default console colors)
                    // If user is hovering over a button, set background as white and text as black, making it look highlighted
                    Console.BackgroundColor = i == j ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = i == j ? ConsoleColor.Black : ConsoleColor.White;
                    Console.WriteLine(menuOptions[j]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                // Reset color and read the key that user presses
                Console.ResetColor();
                ConsoleKey key = Console.ReadKey().Key;
                // Check what user pressed and go up or down in index as long as it is within the length of menuOptions. Enter returns Enum
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        // If the key is the DownArrow, and if user is not hovering over the last menuOptions button, go down the menuOptions array
                        // Without if, it will throw an exception
                        if (i < menuOptions.Length - 1)
                        {
                            i++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        // If the key is the UpArrow, and if user is not hovering over the first menuOptions button, go up the menuOptions array
                        // Without if, it will throw an exception
                        if (i > 0)
                        {
                            i--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // returns the enum value that is indexed at i
                        // Example:
                        //      enum MainMenuOptions { Login = 0, Exit = 1 }
                        //      if (i == 1)
                        //          return MainMenuOption.Exit
                        return (TEnum)Enum.ToObject(typeof(TEnum), i);

                }
            }
        }
    }
}
