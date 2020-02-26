using System;

namespace MagicInventory
{
    // This class manages the main menu of the application.
    public class MainMenu : IMenu
    {
        // Possible user choices.
        public const string OWNER_OPTION = "1";
        public const string FRANCHISE_OPTION = "2";
        public const string CUSTOMER_OPTION = "3";
        public const string EXIT_OPTION = "4";

        // The submenus.
        private IMenu ownerMenu, franchiseMenu, customerMenu;

        // Constructor
        public MainMenu()
        {
            // Creating the submenus.
            ownerMenu = new OwnerMenu();
            franchiseMenu = new FranchiseMenu();
            customerMenu = new CustomerMenu();
        }

        // Starts the menu.
        public bool start()
        {
            var done = false;
            // Loops over until the user decides to exit.
            while (!done)
            {
                // Displays the menu and gets the user's input.
                displayMenu();
                var option = Console.ReadLine();
                bool quit = false;
                switch (option)
                {
                    // If the user chooses a menu option, that menu is started.
                    case OWNER_OPTION:
                        Console.Clear();
                        quit = ownerMenu.start();
                        break;
                    case FRANCHISE_OPTION:
                        Console.Clear();
                        quit = franchiseMenu.start();
                        break;
                    case CUSTOMER_OPTION:
                        Console.Clear();
                        quit = customerMenu.start();
                        break;
                    // If the user chooses the Exit option, the loop ends
                    case EXIT_OPTION:
                        done = true;
                        break;
                    // User entered invalid input.
                    default:
                        Utility.displayError("Error - That was not a valid option. Please enter a number from 1 - 4");
                        break;
                }
                // If, in one of the submenus, the user decided to quit, the loop ends.
                if (quit)
                {
                    done = true;
                }
                Console.Clear();
            }
            // This value doesn't do anything, it just allows this class to implement IMenu
            return true;
        }

        // Helper method to display the menu.
        public void displayMenu()
        {
            Console.WriteLine("Welcome to Marvellous Magic");
            Console.WriteLine("===========================");
            Console.WriteLine("\t1. Owner");
            Console.WriteLine("\t2. Franchise Owner");
            Console.WriteLine("\t3. Customer");
            Console.WriteLine("\t4. Quit");
            Console.Write("Enter an option: ");
        }
    }
}
