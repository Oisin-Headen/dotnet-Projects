using System;

namespace MagicInventory
{
    // A class containing a few utility methods.
    public class Utility
    {
        // Gets either true or false from the user.
        public static bool getTrueFalseChoice(string message)
        {
            Console.Clear();
            bool choice = true;
            bool done = false;
            while (!done)
            {
                Console.Write(message);
                var trueFalseChoice = Console.ReadLine();

                switch (trueFalseChoice)
                {
                    // User is able to enter multiple inputs and get the right answer.
                    case "T":
                    case "true":
                    case "True":
                    case "TRUE":
                        choice = true;
                        done = true;
                        Console.Clear();
                        break;
                    case "F":
                    case "false":
                    case "False":
                    case "FALSE":
                        choice = false;
                        done = true;
                        Console.Clear();
                        break;
                    // User did not enter a valid option.
                    default:
                        Console.Clear();
                        Console.WriteLine("Error - Please choose either True or False");
                        break;
                }
            }
            return choice;
        }

        // Gets an amount of stock from the user.
        public static int getStockAmount()
        {
            bool done = false;
            int quantity = 0;
            // Loops until the user gives a correct amount of stock
            while (!done)
            {
                Console.Write("Enter amount of stock to order: ");
                string userQuantity = Console.ReadLine();
                if (int.TryParse(userQuantity, out quantity))
                {
                    done = true;
                }
                // The user input could not be parsed to an int.
                else
                {
                    displayError("Error - That was not a valid quantity");
                }
            }
            return quantity;
        }

        // Displays an error message.
        public static void displayError(string error)
        {
            // This displays the error message at the top of the next display.
            Console.Clear();
            Console.WriteLine(error);
            Console.WriteLine();
        }
    }
}
