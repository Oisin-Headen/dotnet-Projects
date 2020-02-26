using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicInventory
{
    // This is a simple Stock Request class, containing information about a stock request.
    public class StockRequest
    {
        public string RequestID { get; set; }
        public string Store { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }

        public StockRequest(string requestID, string store, string productID, int quantity)
        {
            RequestID = requestID;
            Store = store;
            ProductID = productID;
            Quantity = quantity;
        }
    }
}
