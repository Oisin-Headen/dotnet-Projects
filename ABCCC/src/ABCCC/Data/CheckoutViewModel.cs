using ABCCC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCC.Data
{
    public class CheckoutViewModel
    {
        public int Price { get; set; }
        public CCInformation CreditCard { get; set; }
    }
}
