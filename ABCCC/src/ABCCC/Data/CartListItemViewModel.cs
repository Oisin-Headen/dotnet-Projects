using ABCCC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCC.Data
{
    public class CartListItemViewModel
    {
        public CineplexMovie CineplexMovie { get; set; }
        public int AdultSeats { get; set; }
        public int ConcessionSeats { get; set; }
    }
}
