using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MagicInventory
{
    // This class of static methods manages all interaction with the Json files.
    public class JsonUtil
    {
        // This method gets the owner stock file.
        public static Dictionary<string, Stock> getOwnerStock()
        {
            string storeFile = File.ReadAllText("data/owners_inventory.json");
            return JsonConvert.DeserializeObject<Dictionary<string, Stock>>(storeFile);
        }

        // This method gets a store stock file.
        public static Dictionary<string, int> getStoreStock(string stockFile)
        {
            string storeFile = File.ReadAllText($"data/{stockFile}_inventory.json");
            return JsonConvert.DeserializeObject<Dictionary<string, int>>(storeFile);
        }

        // This method gets the stock requests file.
        public static Dictionary<string, StockRequest> getStockRequests()
        {
            string storeFile = File.ReadAllText("data/stockrequests.json");
            return JsonConvert.DeserializeObject<Dictionary<string, StockRequest>>(storeFile);
        }

        // This method gets the workshops file.
        public static Dictionary<string, Workshop> getWorkshops()
        {
            string workshopsFile = File.ReadAllText("data/workshops.json");
            return JsonConvert.DeserializeObject<Dictionary<string, Workshop>>(workshopsFile);
        }

        // This method saves the owner stock file.
        public static void saveOwnerStock(Dictionary<string, Stock> ownerMap)
        {
            save(ownerMap, "data/owners_inventory.json");
        }

        // This method saves the stock requests file.
        public static void saveStockRequests(Dictionary<string, StockRequest> requests)
        {
            save(requests, "data/stockrequests.json");
        }

        // This method saves a store stock file.
        public static void saveStoreStock(Dictionary<string, int> storeMap, string store)
        {
            save(storeMap, $"data/{store}_inventory.json");
        }

        // This method saves the workshops file.
        public static void saveWorkshops(Dictionary<string, Workshop> workshops)
        {
            save(workshops, "data/workshops.json");
        }

        // This method is used by the save methods to save something to a specified file.
        private static void save(object itemToSave, string fileName)
        {
            string saveFile = JsonConvert.SerializeObject(itemToSave, Formatting.Indented);
            File.WriteAllText(fileName, saveFile);
        }
    }
}

