using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MagicInventory
{
    // This class handels the Franchise Menu.
    public class FranchiseMenu : IMenu
    {
        // These constants are the options the user can chose on the menu.
        public const string DISPLAY_INVENTORY_OPTION = "1";
        public const string DISPLAY_INVENTORY_THRESHOLD_OPTION = "2";
        public const string ADD_INVENTORY_OPTION = "3";
        public const string RETURN_OPTION = "4";
        public const string EXIT_OPTION = "5";
       
        // The store the user is on.
        private string franchiseID;
        
        // Helper method to display the menu.
        public void displayMenu()
        {
            Console.WriteLine($"Welcome to Marvellous Magic (Franchise Holder - {franchiseID})");
            Console.WriteLine("==============================================================");
            Console.WriteLine("\t1. Display Inventory");
            Console.WriteLine("\t2. Display Inventory (Threshold)");
            Console.WriteLine("\t3. Add New Inventory Item");
            Console.WriteLine("\t4. Return to Main Menu");
            Console.WriteLine("\t5. Exit");
            Console.Write("Enter an option: ");
        }
        
        // Main methos of this class.
        public bool start()
        {
            // Gets the user's store Id, which is help in the franchiseID field.
            getStoreId();
            var quit = false;
            var done = false;
            while (!done)
            {
                displayMenu();
                var option = Console.ReadLine();
                switch (option)
                {
                    // Depending on what option the user chose, this will call a helper method,
                    case DISPLAY_INVENTORY_OPTION:
                        Console.Clear();
                        displayInventory();
                        break;
                    case DISPLAY_INVENTORY_THRESHOLD_OPTION:
                        Console.Clear();
                        displayInventoryThreshold();
                        break;
                    case ADD_INVENTORY_OPTION:
                        Console.Clear();
                        addInventory();
                        break;
                    // ...or end the loop. If they chose to exit, the quit variable is set to true..
                    case RETURN_OPTION:
                        done = true;
                        break;
                    case EXIT_OPTION:
                        done = true;
                        quit = true;
                        break;
                    default:
                        // Invalid input was entered.
                        Utility.displayError("Error - That was not a valid option. Please enter a number from 1 - 5");
                        break;
                }
            }
            // Returns whether or not the user wants to quit.
            return quit;
        }
        
        // Helper method to get the store Id from the user.
        private void getStoreId()
        {
            string userInput;
            bool done = false;
            while (!done)
            {
                Console.Write("Enter your store ID: ");
                userInput = Console.ReadLine();
                // This allows the user to enter a variaty of input and get the appropiate store Id.
                switch (userInput)
                {
                    case "North":
                    case "north":
                    case "N":
                        done = true;
                        franchiseID = "North";
                        break;
                    case "East":
                    case "east":
                    case "E":
                        done = true;
                        franchiseID = "East";
                        break;
                    case "West":
                    case "west":
                    case "W":
                        done = true;
                        franchiseID = "West";
                        break;
                    case "South":
                    case "south":
                    case "S":
                        done = true;
                        franchiseID = "South";
                        break;
                    case "CBD":
                    case "cbd":
                    case "C":
                        done = true;
                        franchiseID = "CBD";
                        break;
                    default:
                        // The user entered invalid input.
                        Utility.displayError("Error - That was not a valid store id");
                        break;
                }
            }
            Console.Clear();
        }
        
        // Helper method to get the restocking threshold.
        private int getRestockThreshold()
        {
            bool done = false;
            int restockThreshold = 0;
            // Loops until the user enters a valid int.
            while (!done)
            {
                Console.Write("Please enter a threshold for restocking: ");
                string restockString = Console.ReadLine();
                // Tries to get the value from the user's input.
                if (int.TryParse(restockString, out restockThreshold))
                {
                    done = true;
                }
                else
                {
                    // The user did not enter a valid int.
                    Utility.displayError($"Error - {restockString} is not a valid restock threshold. Please enter a number");
                }
            }
            Console.Clear();
            return restockThreshold;
        }
        
        // Checks if the user wants to restock, if they have chosen an item they have enough stock of.
        private bool checkIfRestock(int stockLevel, int restockThreshold)
        {
            bool checkRestock = true;
            // If there is more stock than the restock threshold, the user is prompted to ensure they want to proceed.
            if (stockLevel > restockThreshold)
            {
                bool done = false;
                while (!done)
                {
                    Console.WriteLine("You already have sufficiant stock for this item." +
                        " Are you sure you want to request more stock? (Y/N)");
                    string restockChoice = Console.ReadLine();
                    if (restockChoice.Equals("Y"))
                    {
                        // The user does want to restock.
                        done = true;
                    }
                    else if (restockChoice.Equals("N"))
                    {
                        // User does not want to request more stock.
                        Console.Clear();
                        checkRestock = false;
                        done = true;
                    }
                    else
                    {
                        // User did not enter a valid option.
                        Utility.displayError("Error - Please enter either (Y)es or (N)o");
                    }
                }
            }
            // This will be left as true if the user wants more stock, 
            // or if the stock level is less than or equal to the restock threshold.
            return checkRestock;
        }
        
        // Helper method to request stock.
        private void requestStock(Dictionary<string, int> storeStockMap, int restockThreshold, string stockId)
        {
            int stockLevel;
            // Gets the item 
            if (storeStockMap.TryGetValue(stockId, out stockLevel))
            {
                // If the user doesn't want to restock, the method returns.
                if (checkIfRestock(stockLevel, restockThreshold) == false)
                {
                    return;
                }
                int quantity = Utility.getStockAmount();

                // This declaration and the try catch block open the file.
                Dictionary<string, StockRequest> requests;
                try
                {
                    requests = JsonUtil.getStockRequests();
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

                // This section of code checks whether an id is being used, and if it is, increments it, 
                // until a unused value is reached. 
                int requestId = 1;
                while (requests.ContainsKey($"{requestId}"))
                {
                    requestId++;
                }
                // Creates a new request, and adds it to the requests file, and saves.
                StockRequest request = new StockRequest($"{requestId}", franchiseID, stockId, quantity);
                requests.Add(request.RequestID, request);
                JsonUtil.saveStockRequests(requests);
                Console.Clear();
            }
            else
            {
                // A valid stock Id was not passed in.
                Utility.displayError("Error - That is not a valid stock ID");
            }
        }
        
        // The Display Inventory option.
        private void displayInventory()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, int> storeStockMap;
            Dictionary<string, Stock> ownerStock;
            try
            {
                storeStockMap = JsonUtil.getStoreStock(franchiseID);
                ownerStock = JsonUtil.getOwnerStock();
            }
            catch(FileNotFoundException)
            {                
                // If the file isn't found, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be found.");
                return;
            }
            catch(JsonReaderException)
            {
                // If the Json is corrupted, this method cannot run.
                Utility.displayError("A file necessary for that operation could not be read.");
                return;
            }
            // Gets the restock threshold from the user.
            int restockThreshold = getRestockThreshold();
            string header = $"{"Id",-5}{"Name",-25}{"Stock Level",-15}{"Re-Stock",-10}";
            Console.WriteLine(header);
            Console.WriteLine(new String('=', header.Length));
            // Uses the owner file to get the name, and displays the stock information.
            foreach (var stockId in storeStockMap.Keys)
            {
                Console.WriteLine($"{stockId,-5}{ownerStock[stockId].Name,-25}{storeStockMap[stockId],-15}" +
                    $"{storeStockMap[stockId] <= restockThreshold,-10}");
            }
            Console.WriteLine();
            Console.Write("Enter item to request (Press Enter to return to menu): ");
            string userInput = Console.ReadLine();
            if (userInput.Equals(""))
            {
                // Empty string is the enter key, meaning return to menu
                Console.Clear();
            }
            else
            {
                // Errors in the input are handled in this helper method.
                requestStock(storeStockMap, restockThreshold, userInput);
            }
        }

        // The Display Inventory Threshold option.
        private void displayInventoryThreshold()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, int> storeStockMap;
            Dictionary<string, Stock> ownerStock;
            try
            {
                storeStockMap = JsonUtil.getStoreStock(franchiseID);
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
            // A list of Ids that can be chosen.
            IList<string> validIDs = new List<string>();
            // Gets the restock threshold from the user.
            int restockThreshold = getRestockThreshold();
            string header = $"{"Id",-5}{"Name",-25}{"Stock Level",-15}{"Re-Stock",-10}";
            Console.WriteLine(header);
            Console.WriteLine(new String('=', header.Length));
            // As with the previous method, this loops through the keys and uses the owner 
            // file to get information about the stock items.
            foreach (var key in storeStockMap.Keys)
            {
                // The results are filtered on the restock threshold.
                if (storeStockMap[key] <= restockThreshold)
                {
                    Console.WriteLine($"{key,-5}{ownerStock[key].Name,-25}{storeStockMap[key],-15}" +
                        $"{storeStockMap[key] <= restockThreshold,-10}");
                    // This was a valid item, so its Id is added to the valid Id list.
                    validIDs.Add(key);
                }
            }
            Console.WriteLine();
            Console.Write("Enter item to request (Press Enter to return to menu): ");
            string userInput = Console.ReadLine();
            if (userInput.Equals(""))
            {
                // Empty string is the enter key, meaning return to menu
                Console.Clear();
            }
            else if (validIDs.Contains(userInput))
            {
                // This is a valid Id, so reuse the requestStock method.
                requestStock(storeStockMap, restockThreshold, userInput);
            }
            else
            {
                // The user did not enter a valid Id.
                Utility.displayError("Error - That is not a valid stock ID");
            }
        }

        // The Add New Inventory Item option.
        private void addInventory()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, int> storeStockMap;
            Dictionary<string, Stock> ownerStockMap;
            try
            {
                storeStockMap = JsonUtil.getStoreStock(franchiseID);
                ownerStockMap = JsonUtil.getOwnerStock();
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
            // A list of Ids that can be added.
            IList<string> validIDs = new List<string>();

            string header = $"{"Id",-5}{"Name",-25}";

            Console.WriteLine(header);
            Console.WriteLine(new string('=', header.Length));
            // Goes through the owner's stock, and checks if it is contained in this store. 
            //If it isn't, it is displayed, and added to the valid Ids.
            foreach (var item in ownerStockMap.Values)
            {
                if (!storeStockMap.ContainsKey(item.Id))
                {
                    Console.WriteLine($"{item.Id,-5}{item.Name,-25}");
                    validIDs.Add(item.Id);
                }
            }
            Console.WriteLine();
            Console.Write("Enter item to request (Press Enter to return to menu): ");
            string stockId = Console.ReadLine();
            if (stockId.Equals(""))
            {
                // Empty string is the enter key, meaning return to menu
                Console.Clear();
            }
            else if (validIDs.Contains(stockId))
            {
                // Adds the chosen item to the store with a quantity of zero.
                storeStockMap.Add(stockId, 0);
                // Re-using the requestStock function. This will create a new stock request and add it to the file.
                requestStock(storeStockMap, 0, stockId);

                // Updating the Store's inventory with the zero quantity item.
                JsonUtil.saveStoreStock(storeStockMap, franchiseID);
                Console.Clear();
            }
            else
            {
                // User entered invalid input.
                Utility.displayError("Error - That is not a valid stock ID");
            }
        }
    }
}