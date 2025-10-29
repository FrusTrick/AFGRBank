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
        /// <param name="questionText">A string displayed at the top of the menu</param>
        /// <param name="menuOptions">Options in an array that is displayed to the user</param>
        /// <returns>Returns the index chosen by user</returns>
        public static int ReadOptionIndex<T>(string questionText, T[] menuOptions)
        {
            int i = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(questionText + "\n");
                for (int j = 0; j < menuOptions.Length; j++)
                {
                    Console.BackgroundColor = i == j ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = i == j ? ConsoleColor.Black : ConsoleColor.White;
                    Console.WriteLine(menuOptions[j]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (i < menuOptions.Length - 1)
                        {
                            i++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (i > 0)
                        {
                            i--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // returns index as int
                        return i;

                }
            }
        }



        /// <summary>
        /// Displays a menu with options for the user to choose from. They can navigate using arrow keys up and down. Clears console before displaying.
        /// </summary>
        /// <typeparam name="T">An array that is going to be displayed as strings</typeparam>
        /// <typeparam name="TEnum">An enum that represents options in menu, where each index is an option</typeparam>
        /// <param name="questionText">A string displayed at the top of the menu</param>
        /// <param name="menuOptions">Options in an array that is displayed to the user</param>
        /// <returns>Returns an Enum with chosen enum index</returns>
        public static TEnum ReadOption<T, TEnum>(string questionText, T[] menuOptions) where TEnum : Enum
        {
            int i = 0;
            while (true)
            {
                Console.Clear();
                // Write out question and display options, currently selected index gets highlighted
                Console.WriteLine(questionText + "\n");
                for (int j = 0; j < menuOptions.Length; j++)
                {
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
                        if (i < menuOptions.Length - 1)
                        {
                            i++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (i > 0)
                        {
                            i--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // returns specified enum
                        return (TEnum)Enum.ToObject(typeof(TEnum), i);

                }
            }
        }
    }
}
