using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABCCC.Models
{
    public partial class CineplexMovie
    {
        public enum DayOfWeek
        {
            Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
        }
        public enum TimePeriod
        {
            am, pm
        }

        public CineplexMovie()
        {
            AvailableSeats = 20;
        }

        public int CineplexId { get; set; }
        public int MovieId { get; set; }
        public DayOfWeek Day { get; set; }
        [Range(1, 12)]
        public int Hour { get; set; }
        public TimePeriod Period { get; set; }
        public int AvailableSeats { get; set; }

        public virtual Cineplex Cineplex { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
