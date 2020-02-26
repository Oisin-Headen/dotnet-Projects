using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicInventory
{
    // This is a simple stock Class, containing information about a stock item.
    public class Stock
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int StockLevel { get; set; }
        public int Price { get; set; }
    }
}
