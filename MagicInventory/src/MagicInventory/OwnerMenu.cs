using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MagicInventory
{
    // This class manages the OwnerMenu.
    public class OwnerMenu : IMenu
    {
        // These constants are the options the user can choose.
        public const string DISPLAY_STOCK_OPTION = "1";
        public const string DISPLAY_STOCK_TRUE_FALSE_OPTION = "2";
        public const string DISPLAY_PRODUCT_OPTION = "3";
        public const string RETURN_OPTION = "4";
        public const string EXIT_OPTION = "5";

        // Helper method to display the menu.
        public void displayMenu()
        {
            Console.WriteLine("Welcome to Marvellous Magic (Owner)");
            Console.WriteLine("=========================================");
            Console.WriteLine("\t1. Display All Stock Requests");
            Console.WriteLine("\t2. Display Stock Requests (True/False)");
            Console.WriteLine("\t3. Display All Product Lines");
            Console.WriteLine("\t4. Return to Main Menu");
            Console.WriteLine("\t5. Exit");
            Console.Write("Enter an option: ");
        }

        // Starts the Menu.
        public bool start()
        {
            var done = false;
            var quit = false;
            while (!done)
            {
                displayMenu();
                var option = Console.ReadLine();
                switch (option)
                {
                    // Calls the appropriate method.
                    case DISPLAY_STOCK_OPTION:
                        Console.Clear();
                        displayAllRequests();
                        break;
                    case DISPLAY_STOCK_TRUE_FALSE_OPTION:
                        Console.Clear();
                        displayStock();
                        break;
                    case DISPLAY_PRODUCT_OPTION:
                        Console.Clear();
                        displayProducts();
                        break;
                    // Ends the loop.
                    case RETURN_OPTION:
                        done = true;
                        break;
                    case EXIT_OPTION:
                        done = true;
                        quit = true;
                        break;
                    default:
                        // User entered invalid input.
                        Utility.displayError("Error - That was not a valid option.Please enter a number from 1 - 5");
                        break;
                }
            }
            return quit;
        }

        // Helper method to check if the owner has enough stock.
        private bool checkOwnerStock(Dictionary<string, Stock> stockMap, StockRequest request)
        {
            Stock item;
            if (stockMap.TryGetValue(request.ProductID, out item))
            {
                if (item.StockLevel >= request.Quantity)
                {
                    // If the owner has enough stock, true is returned.
                    return true;
                }
            }
            return false;
        }

        // Helper method to process a stock request.
        private void process(Dictionary<string, StockRequest> requestMap, 
            Dictionary<string, Stock> ownerMap, string requestToProcess)
        {
            StockRequest selectedRequest;
            // Gets the value from the request map.
            if (requestMap.TryGetValue(requestToProcess, out selectedRequest))
            {
                // Ensures that the product exists.
                Stock product;
                if (ownerMap.TryGetValue(selectedRequest.ProductID, out product))
                {
                    // Ensures that the owner hase enough stock.
                    if (product.StockLevel >= selectedRequest.Quantity)
                    {
                        // This declaration and the try catch block open the file.
                        Dictionary<string, int> storeMap;
                        try
                        {
                            storeMap = JsonUtil.getStoreStock(selectedRequest.Store);
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

                        int storeStockLevel = storeMap[selectedRequest.ProductID];
                        // Updates the values of all relevent files.
                        product.StockLevel -= selectedRequest.Quantity;
                        storeStockLevel += selectedRequest.Quantity;
                        requestMap.Remove(selectedRequest.RequestID);
                        // Saves the files.
                        JsonUtil.saveOwnerStock(ownerMap);
                        JsonUtil.saveStoreStock(storeMap, selectedRequest.Store);
                        JsonUtil.saveStockRequests(requestMap);

                        Console.Clear();

                    }
                    // There was not enough stock left.
                    else
                    {
                        Utility.displayError("There is not enough stock to fulfil that request.");
                    }
                }
            }
            // The user did not enter a valid Id.
            else
            {
                Utility.displayError("Error - That was not a valid Request ID");
            }
        }

        // Displays all the stock requests.
        private void displayAllRequests()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, StockRequest> stockRequestMap;
            Dictionary<string, Stock> ownerStockMap;
            try
            {
                stockRequestMap = JsonUtil.getStockRequests();
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

            Console.WriteLine($"{"ID",-5}{"Store",-10}{"Product",-25}{"Quantity",-10}" +
                $"{"Current Stock",-15}{"Stock Availability",-20}");
            // Loops over the requests and displays them.
            foreach (var request in stockRequestMap.Values)
            {
                bool available = checkOwnerStock(ownerStockMap, request);
                Stock product;
                if (ownerStockMap.TryGetValue(request.ProductID, out product))
                {
                    Console.WriteLine($"{request.RequestID,-5}{request.Store,-10}{product.Name,-25}{request.Quantity,-10}" +
                        $"{product.StockLevel,-15}{available,-20}");
                }
            }
            Console.WriteLine();
            Console.Write("Enter Request to process (enter to return to menu): ");
            var requestToProcess = Console.ReadLine();
            if (requestToProcess.Equals(""))
            {
                // Empty string is the enter key, meaning return to menu
                Console.Clear();
            }
            else
            {
                // The request is processed.
                process(stockRequestMap, ownerStockMap, requestToProcess);
            }
        }

        // Displays either all the requests that can be fufilled, or all the ones that can't.
        private void displayStock()
        {
            // These declarations and the try catch block open the files.
            Dictionary<string, StockRequest> stockRequestMap;
            Dictionary<string, Stock> ownerStockMap;
            try
            {
                stockRequestMap = JsonUtil.getStockRequests();
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
            // A list of valid Ids.
            IList<string> validIDs = new List<string>();
            // User is asked if they want to see an availability of true or false.
            bool viewAvailability = Utility.getTrueFalseChoice("View true/false availability: ");

            Console.WriteLine($"{"ID",-5}{"Store",-10}{"Product",-25}{"Quantity",-10}" +
                $"{"Current Stock",-15}{"Stock Availability",-20}");
            // Loops over the requests, and displays information about them.
            foreach (var request in stockRequestMap.Values)
            {
                bool available = checkOwnerStock(ownerStockMap, request);
                Stock product;
                if (ownerStockMap.TryGetValue(request.ProductID, out product))
                {
                    // Now the results are filtered by availability.
                    if (available == viewAvailability)
                    {
                        Console.WriteLine($"{request.RequestID,-5}{request.Store,-10}{product.Name,-25}{request.Quantity,-10}" +
                            $"{product.StockLevel,-15}{available,-20}");
                        // The valid Ids are filtered based on the availability.
                        validIDs.Add(request.RequestID);
                    }
                }
            }
            Console.WriteLine();
            Console.Write("Enter Request to process (enter to return to menu): ");
            var requestToProcess = Console.ReadLine();
            // If the Id is valid, then the request is processed.
            if (validIDs.Contains(requestToProcess))
            {
                process(stockRequestMap, ownerStockMap, requestToProcess);
            }
            else if (requestToProcess.Equals(""))
            {
                // Empty string is the enter key, meaning return to menu
                Console.Clear();
            }
            // User entered invalid input.
            else
            {
                Utility.displayError("Error - That is not a valid Request ID");
            }
        }

        // Displays all product lines.
        private void displayProducts()
        {
            // This declaration and the try catch block open the file.
            Dictionary<string, Stock> stockMap;
            try
            {
                stockMap = JsonUtil.getOwnerStock();
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

            string header = $"{"Id",-5}{"Name",-25}{"Stock Level",-15}";

            Console.WriteLine(header);
            Console.WriteLine(new String('=', header.Length));
            // Loops over the products and displays them.
            foreach (var item in stockMap.Values)
            {
                Console.WriteLine($"{item.Id,-5}{item.Name,-25}{item.StockLevel,-15}");
            }
            Console.WriteLine();
            // Buffer to stop the screen from leaving until the user presses enter.
            Console.WriteLine("(Press enter to exit)");
            Console.ReadLine();
            Console.Clear();
        }
    }
}