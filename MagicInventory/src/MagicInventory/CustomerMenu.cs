using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MagicInventory
{
    // Class that manages the Customer Menu.
    public class CustomerMenu : IMenu
    {
        // These constants are the options the user can choose on the menu.
        public const string DISPLAY_PRODUCTS_OPTION = "1";
        public const string DISPLAY_WORKSHOPS_OPTION = "2";
        public const string RETURN_OPTION = "3";
        public const string EXIT_OPTION = "4";
        // These constants are the options the user can select when choosing the store.
        public const string NORTH = "1";
        public const string EAST = "2";
        public const string WEST = "3";
        public const string SOUTH = "4";
        public const string CBD = "5";
        // This is the number of items that can appear on a page.
        public const int PAGE_ITEMS = 5;
        // This is the discount applied to a transaction if the user books into a workshop.
        public const double WORKSHOP_DISCOUNT = 0.9;
        // This field holds the store that the user is ordering from.
        private string store;

        // The string is the stock ID, and the int is quantity to order.
        private Dictionary<string, int> cart = new Dictionary<string, int>();
        // Each workshop can only be booked once.
        private IList<string> bookingIds = new List<string>();
        // A list of workshops that the user can book.
        private IList<string> validWorkshopIds = new List<string>();

        // Simple function to display the Customer Menu
        public void displayMenu()
        {
            Console.WriteLine($"Welcome to Marvellous Magic (Retail - {store})");
            Console.WriteLine("===========================================");
            Console.WriteLine("\t1. Display Products");
            Console.WriteLine("\t2. Display Workshops");
            Console.WriteLine("\t3. Return to Main Menu");
            Console.WriteLine("\t4. Exit");
            Console.Write("Enter an option: ");
        }

        public bool start()
        {
            // Gets the store the user is "visiting".
            getStore();
            var quit = false;
            var done = false;
            // Loops over the menu until the user decides to leave.
            while (!done)
            {
                displayMenu();
                // Gets the user's input, and handles it in a switch.
                var option = Console.ReadLine();
                switch (option)
                {
                    // When the user chooses a function, the appropiate helper method is called.
                    case DISPLAY_PRODUCTS_OPTION:
                        Console.Clear();
                        displayProducts();
                        break;
                    case DISPLAY_WORKSHOPS_OPTION:
                        Console.Clear();
                        displayWorkshops();
                        break;
                    // When the user decides to leave, the loop ends, and the return value may be changed.
                    case RETURN_OPTION:
                        done = true;
                        break;
                    case EXIT_OPTION:
                        done = true;
                        quit = true;
                        break;
                    default:
                        Utility.displayError("Error - That was not a valid option. Please enter a number from 1 - 4");
                        break;
                }
            }
            return quit;
        }

        // Helper function to allow the user to choose a store. The result is put in the store field.
        private void getStore()
        {
            bool done = false;
            // Loops over the code until the user gives a valid store
            while (!done)
            {
                Console.WriteLine("Magic Inventory Stores:");
                Console.WriteLine("\t 1. North");
                Console.WriteLine("\t 2. East");
                Console.WriteLine("\t 3. West");
                Console.WriteLine("\t 4. South");
                Console.WriteLine("\t 5. CDB");
                Console.Write("Please choose a store to order from: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case NORTH:
                        store = "North";
                        done = true;
                        break;
                    case EAST:
                        store = "East";
                        done = true;
                        break;
                    case WEST:
                        store = "West";
                        done = true;
                        break;
                    case SOUTH:
                        store = "South";
                        done = true;
                        break;
                    case CBD:
                        store = "CBD";
                        done = true;
                        break;
                    default:
                        // The user did not enter a valid choice, so an error is displayed.
                        Utility.displayError("Error - That is not a valid choice. Please enter a number from 1 to 5.");
                        break;
                }
            }
            Console.Clear();
        }

        // Helper function to display a page of stock.
        private void displayStockPage(IList<string> storeStock, int page,
            Dictionary<string, Stock> ownerStock, Dictionary<string, int> storeStockMap)
        {
            // Writes the top of the page.
            string header = $"{"ID",-5}{"Product",-25}{"Current Stock",-15}";
            Console.WriteLine(header);
            Console.WriteLine(new string('=', header.Length));
            // Calculates the starting item, with the page number and the number of items per page.
            int startPage = page * PAGE_ITEMS;
            // End page is five items later.
            int endPage = startPage + PAGE_ITEMS;
            // Ensures that endPage does not overflow the list.
            if (endPage > storeStock.Count)
            {
                endPage = storeStock.Count;
            }
            // If startPage ends up larger than endPage, this code will not run.
            for (int i = startPage; i < endPage; i++)
            {
                // Uses the stock Id to get the name from the owner's inventory.
                Console.WriteLine($"{storeStock[i],-5}{ownerStock[storeStock[i]].Name,-25}" +
                    $"{storeStockMap[storeStock[i]],-15}");
            }
            Console.WriteLine();
            Console.WriteLine("[Legend: 'P' Next Page | 'R' Return to Menu | 'C' Complete Transaction]");
        }

        // This method adds an item to the cart field, or, if it already 
        private bool addToCart(string stock, Dictionary<string, int> storeStockMap)
        {
            // Gets the amount the user wants.
            int quantity = Utility.getStockAmount();
            // If the cart already contains that stock ID, then the amount is added to the cart quantity.
            if (cart.ContainsKey(stock))
            {
                int cartQuantity = cart[stock];
                if ((cartQuantity + quantity) > storeStockMap[store])
                {
                    return false;
                }
                cart[stock] = cartQuantity + quantity;
            }
            // If the Id doesn't exist in the cart, it adds the Id and quantity.
            else if (quantity <= storeStockMap[stock])
            {
                cart.Add(stock, quantity);
            }
            // If there is not enough quantity in the store, the item is not able to be added to the cart.
            else
            {
                return false;
            }
            return true;
        }

        // This method displays the cart and bookings, the price of the transaction, 
        // and asks them if they would like to proceed. Whether or not they do, the cart is cleared.
        private void completeTransaction()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, int> storeStock;
            Dictionary<string, Workshop> workshops;
            Dictionary<string, Stock> ownerStock;
            try
            {
                storeStock = JsonUtil.getStoreStock(store);
                workshops = JsonUtil.getWorkshops();
                ownerStock = JsonUtil.getOwnerStock();
            }
            catch (FileNotFoundException)
            {
                // If the file isn't found, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be found.");
                return;
            }
            catch (JsonReaderException)
            {
                // If the Json is corrupted, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be read.");
                return;
            }
            // These Dividers space out the transaction summary.
            var largeDivider = new string('=', 45);
            var smallDivider = new string('-', 45);
            double price = 0;
            // The cart summary is only displayed if there is an item in the cart.
            if (cart.Count != 0)
            {
                Console.WriteLine(largeDivider);
                Console.WriteLine("Cart:");
                Console.WriteLine(smallDivider);
                Console.WriteLine($"{"Name",-25}{"Quantity",-10}");
                Console.WriteLine(smallDivider);
                // For each key, this uses the owner stock to display the name, and add the price.
                foreach (var stockId in cart.Keys)
                {
                    Console.WriteLine($"{ownerStock[stockId].Name,-25}{cart[stockId],-10}");
                    price += ownerStock[stockId].Price;
                    // This change will not be saved unless user completes the transaction
                    storeStock[stockId] -= cart[stockId];
                }
            }
            Console.WriteLine(largeDivider);
            // The bookings summary is only displayed if there is a booking reserved.
            if (bookingIds.Count != 0)
            {
                Console.WriteLine("Bookings:");
                Console.WriteLine(smallDivider);
                Console.WriteLine($"{"Name",-40}{"Date",-10}");
                Console.WriteLine(smallDivider);
                // For each Id, this uses the workshops file to display the name and the date of the workshop.
                foreach (var bookingId in bookingIds)
                {
                    Console.WriteLine($"{workshops[bookingId].Name,-40}{workshops[bookingId].Date,-10}");
                    // This change will not be saved unless user completes the transaction
                    workshops[bookingId].SeatsLeft -= 1;
                }
                Console.WriteLine(largeDivider);
                // If there was a booking, the price is discounted.
                price = price * WORKSHOP_DISCOUNT;
            }
            Console.WriteLine();
            Console.WriteLine($"Total Price: {price}");
            Console.WriteLine();
            bool done = false;
            while (!done)
            {
                Console.Write("Do you want to complete this transation? (Y/N): ");
                string userInput = Console.ReadLine();
                if (userInput.Equals("Y"))
                {
                    // Saves the files, gives the user a unique booking reference and returns to menu.
                    JsonUtil.saveStoreStock(storeStock, store);
                    JsonUtil.saveWorkshops(workshops);
                    Console.Clear();
                    if (bookingIds.Count > 0)
                    {
                        giveBookingRef();
                    }
                    done = true;
                }
                else if (userInput.Equals("N"))
                {
                    // Returns to menu.
                    Console.Clear();
                    return;
                }
                else
                {
                    // Invalid input, asks again.
                    Console.WriteLine("Error - That was not an option");
                }
            }
            // Whether the user orders or not, the cart is cleared. 
            cart.Clear();
            bookingIds.Clear();
        }

        // Helper method to give the user a booking reference number.
        private void giveBookingRef()
        {
            Console.WriteLine($"Thank you for shopping with us. Your unique booking reference is: {DateTime.Now.Ticks}");
            Console.WriteLine();
        }

        // Helper method for the display products option.
        private void displayProducts()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, int> storeStockMap;
            Dictionary<string, Stock> ownerStock;
            try
            {
                storeStockMap = JsonUtil.getStoreStock(store);
                ownerStock = JsonUtil.getOwnerStock();
            }
            catch (FileNotFoundException)
            {
                // If the file isn't found, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be found.");
                return;
            }
            catch (JsonReaderException)
            {
                // If the Json is corrupted, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be read.");
                return;
            }

            bool done = false;
            int page = 0;
            IList<string> storeStock = new List<string>(storeStockMap.Keys);
            // Loops until the user is finished.
            while (!done)
            {
                // Displays the current page.
                displayStockPage(storeStock, page, ownerStock, storeStockMap);
                Console.Write("Enter Item Number to purchase or Function: ");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    // If the user wants the next page, the page variable is incremented.
                    case "P":
                        page++;
                        Console.Clear();
                        break;
                    // The user ends  the loop.
                    case "R":
                        done = true;
                        Console.Clear();
                        break;
                    // The user wants to complete the transaction, calls the helper method.
                    case "C":
                        Console.Clear();
                        completeTransaction();
                        done = true;
                        break;
                    // If none of the above work, then the user has entered an Id, or invalid input.
                    default:
                        int stockLevel;
                        if (storeStockMap.TryGetValue(userInput, out stockLevel))
                        {
                            if (!addToCart(userInput, storeStockMap))
                            {
                                // If the choosen item could not be added to the cart, an error is displayed.
                                Utility.displayError("Error - That item could not be added to the cart");
                            }
                            else
                            {
                                // Everything worked fine.
                                Console.Clear();
                            }
                        }
                        else
                        {
                            // If a stockLevel connot be retreived from the store stock, then the user entered invalid input.
                            Utility.displayError("Error - That stock ID does not exist");
                        }
                        break;
                }
            }
        }

        // Helper method for the display workshops option.
        private void displayWorkshops()
        {
            // This declaration and the try catch block open the file.
            Dictionary<string, Workshop> workshops;
            try
            {
                workshops = JsonUtil.getWorkshops();
            }
            catch (FileNotFoundException)
            {
                // If the file isn't found, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be found.");
                return;
            }
            catch (JsonReaderException)
            {
                // If the Json is corrupted, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be read.");
                return;
            }

            bool done = false;
            // Loops until the user is finished.
            while (!done)
            {
                string header = $"{"ID",-5}{"Name",-40}{"Date",-15}";
                Console.WriteLine(header);
                Console.WriteLine(new string('=', header.Length));
                foreach (var workshop in workshops.Values)
                {
                    // Only displays workshops for this store.
                    if (workshop.Store.Equals(store))
                    {
                        // Only displays workshops with seats left.
                        if (workshop.SeatsLeft > 0)
                        {
                            Console.WriteLine($"{workshop.Id,-5}{workshop.Name,-40}{workshop.Date,-15}");
                            // The displayed workshop is a valid choice.
                            validWorkshopIds.Add(workshop.Id);
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine("[Legend: 'R' Return to Menu | 'C' Complete Transaction]");
                Console.WriteLine();
                Console.Write("Enter Workshop to book or Function: ");
                string userInput = Console.ReadLine();
                // If the user input is in the valid Ids, then they are trying to add a workshop.
                if (validWorkshopIds.Contains(userInput))
                {
                    // If the user has already booked into this session, they are not able to again.
                    if (bookingIds.Contains(userInput))
                    {
                        Utility.displayError("You have already added that booking to your cart");
                    }
                    // Otherwise, the workshop is added to their bookings.
                    else
                    {
                        bookingIds.Add(userInput);
                        Console.Clear();
                    }
                }
                // User wants to return to menu.
                else if (userInput.Equals("R"))
                {
                    Console.Clear();
                    done = true;
                }
                // User wants to complete the transaction.
                else if (userInput.Equals("C"))
                {
                    Console.Clear();
                    completeTransaction();
                    done = true;
                }
                // User entered invalid input.
                else
                {
                    Utility.displayError("Error - That was not a valid option");
                }
            }
        }
    }
}