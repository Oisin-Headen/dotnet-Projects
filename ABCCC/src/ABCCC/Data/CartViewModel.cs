using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCC.Data
{
    public class CartViewModel
    {
        public IList<CartListItemViewModel> Items { get; set; }
        public int Price;
    }
}
