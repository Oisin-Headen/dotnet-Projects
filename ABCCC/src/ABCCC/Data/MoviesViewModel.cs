using ABCCC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCC.Data
{
    public class MoviesViewModel
    {
        public IEnumerable<Movie> Movies { get; }
        public IEnumerable<MovieComingSoon> MoviesComingSoon { get; }

        public MoviesViewModel(IEnumerable<Movie> movies, IEnumerable<MovieComingSoon> movieComingSoon)
        {
            Movies = movies;
            MoviesComingSoon = movieComingSoon;
        }
    }
}
