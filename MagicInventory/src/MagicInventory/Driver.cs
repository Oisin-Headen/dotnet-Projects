using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicInventory
{
    // The driver class.
    public class Driver
    {
        public static void Main(string[] args)
        {
            // Creates the MainMenu and starts it.
            var menu = new MainMenu();
            menu.start();
        }
    }
}
