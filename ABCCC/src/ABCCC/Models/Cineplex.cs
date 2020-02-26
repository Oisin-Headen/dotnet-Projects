using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ABCCC.Models
{
    public partial class Cineplex
    {
        public int CineplexId { get; set; }
        public string Location { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public IList<CineplexMovie> CineplexMovie { get; set; }
    }
}
