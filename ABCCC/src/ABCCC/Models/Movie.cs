using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ABCCC.Models
{
    public partial class Movie : AbstractMovie
    {
        [JsonIgnore]
        public IList<CineplexMovie> CineplexMovie { get; set; }
    }
}
