using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCC.Models
{
    public class MovieBooking
    {
        public MovieBooking() { }

        [JsonIgnore]
        public int BookingId { get; set; }
        [JsonIgnore]
        public int OrderId { get; set; }
        [JsonIgnore]
        public virtual Transaction Transaction { get; set; }

        public int CineplexId { get; set; }
        public CineplexMovie.DayOfWeek Day { get; set; }
        public int Hour { get; set; }
        public int AdultSeats { get; set; }
        public int ConcessionSeats { get; set; }
        public CineplexMovie.TimePeriod Period { get; set; }

        [JsonIgnore]
        public virtual Cineplex Cineplex { get; set; }
    }
}
