using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABCCC.Models;
using ABCCC.Controllers;
using ABCCC.WebAPI;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ABCCC.Test
{
    public class Tests
    {
        private ABCCCDataContext _context { get; set; }
        private ABCCCController _apiController { get; set; }
        private EventsController _eventsController { get; set; }
        private CineplexesController _cineplexesController { get; set; }
        private MoviesController _moviesController { get; set; }

        public Tests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ABCCCDataContext>();
            optionsBuilder.UseInMemoryDatabase();
            _context = new ABCCCDataContext(optionsBuilder.Options);

            _context.AllMovies.Add(new Movie()
            {
                Title = "Illinois Jones: Raiders of the Lost Cineplex",
                MovieId = 1
            });
            _context.AllMovies.Add(new Movie()
            {
                Title = "Stone Wars: Attack of the Sticks",
                MovieId = 2
            });
            _context.AllMovies.Add(new Movie()
            {
                Title = "The Horribly Boring Movie",
                MovieId = 10
            });

            _context.Cineplex.Add(new Cineplex()
            {
                CineplexId = 1,
                Location = "Uluru",
                ShortDescription = "A Great Red Rock in the middle of the desert.",
                LongDescription = "It seems that there is a cinema here."
            });
            _context.Cineplex.Add(new Cineplex()
            {
                CineplexId = 2,
                Location = "Petra",
                ShortDescription = "An ancient rock city.",
                LongDescription = "What!? There's a cinema here!?"
            });
            _context.Cineplex.Add(new Cineplex()
            {
                CineplexId = 10,
                Location = "Antarctica",
                ShortDescription = "The only cinema on the icy south continent.",
                LongDescription = "Since it's so isolated, we haven't even put movies here yet."
            });
            _context.Database.EnsureDeleted();
            _context.SaveChanges();

            _context.CineplexMovie.Add(new CineplexMovie()
            {
                CineplexId = 1,
                MovieId = 1,
                Day = CineplexMovie.DayOfWeek.Monday,
                Hour = 4,
                Period = CineplexMovie.TimePeriod.pm
            });
            _context.CineplexMovie.Add(new CineplexMovie()
            {
                CineplexId = 2,
                MovieId = 1,
                Day = CineplexMovie.DayOfWeek.Monday,
                Hour = 4,
                Period = CineplexMovie.TimePeriod.pm
            });
            _context.CineplexMovie.Add(new CineplexMovie()
            {
                CineplexId = 2,
                MovieId = 1,
                Day = CineplexMovie.DayOfWeek.Tuesday,
                Hour = 4,
                Period = CineplexMovie.TimePeriod.pm
            });
            _context.CineplexMovie.Add(new CineplexMovie()
            {
                CineplexId = 1,
                MovieId = 2,
                Day = CineplexMovie.DayOfWeek.Tuesday,
                Hour = 4,
                Period = CineplexMovie.TimePeriod.pm
            });
            _context.SaveChanges();

            _apiController = new ABCCCController(_context);
            _eventsController = new EventsController(_context);
            _cineplexesController = new CineplexesController(_context);
            _moviesController = new MoviesController(_context);
        }


        [Fact]
        public async void API_Distinct_Items_Test()
        {
            var result = await _apiController.GetDistinctCineplexes();

            // Fails if Cineplex 2 is included twice. Cineplexes must be distinct.
            Assert.NotNull(result.SingleOrDefault(r => r.CineplexId == 2));
            // Fails if Cineplex 10 is included at all. This method only includes cineplexes that have movies.
            Assert.Null(result.SingleOrDefault(r => r.CineplexId == 10));

            var moviesResult = await _apiController.GetDistinctMovies();
            // Fails if Movie 2 is included twice. Movies must be distinct.
            Assert.NotNull(moviesResult.SingleOrDefault(r => r.MovieId == 1));
            // Fails if Cineplex 10 is included at all. This method only includes cineplexes that have movies.
            Assert.Null(moviesResult.SingleOrDefault(r => r.MovieId == 10));
        }

        [Fact]
        public async void API_Get_CineplexMovie_Test()
        {
            var result = await _apiController.GetCineplexMovie(1, CineplexMovie.DayOfWeek.Monday, 4, CineplexMovie.TimePeriod.pm);

            Assert.IsType<OkObjectResult>(result);

            var value = ((OkObjectResult)result).Value;
            Assert.IsType<CineplexMovie>(value);
            Assert.Equal(1, ((CineplexMovie) value).MovieId);

            result = await _apiController.GetCineplexMovie(10, CineplexMovie.DayOfWeek.Monday, 4, CineplexMovie.TimePeriod.pm);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1, 1, CineplexMovie.DayOfWeek.Monday, 4, CineplexMovie.TimePeriod.pm)]
        [InlineData(2, 1, CineplexMovie.DayOfWeek.Monday, 4, CineplexMovie.TimePeriod.pm)]
        [InlineData(2, 1, CineplexMovie.DayOfWeek.Tuesday, 4, CineplexMovie.TimePeriod.pm)]
        [InlineData(1, 2, CineplexMovie.DayOfWeek.Tuesday, 4, CineplexMovie.TimePeriod.pm)]
        public async void API_Sessions_Test(int cineplexId, int movieId, CineplexMovie.DayOfWeek day, int hour, CineplexMovie.TimePeriod period)
        {
            var sessions = await _apiController.GetSessions(cineplexId, movieId);
            var session = sessions.SingleOrDefault(cm => cm.Day == day && 
                cm.Hour == hour && 
                cm.Period == period);
            Assert.NotNull(session);
        }

        [Fact]
        public async void Movies_Controller_Test()
        {
            // Ordinary details
            var result = await _moviesController.Details(1);
            Assert.IsNotType<NotFoundResult>(result);
            // Movies not showing in a cineplex should still be displayed
            result = await _moviesController.Details(10);
            Assert.IsNotType<NotFoundResult>(result);

            result = await _moviesController.Details(null);
            Assert.IsType<NotFoundResult>(result);

            result = await _moviesController.Details(20);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Cineplexes_Controller_Test()
        {
            var result = await _cineplexesController.Details(1);
            // ordinary cineplexes
            Assert.IsNotType<NotFoundResult>(result);
            // Cineplexes not showing any movies should still be displayed
            result = await _cineplexesController.Details(10);
            Assert.IsNotType<NotFoundResult>(result);

            result = await _cineplexesController.Details(null);
            Assert.IsType<NotFoundResult>(result);

            result = await _cineplexesController.Details(20);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Enquiry_Post_Test()
        {
            await _eventsController.Create(new Enquiry() { Email = "Someone@Something.com", Message = "A message" });
            Assert.Contains("Someone@Something.com", _context.Enquiry.Select(e => e.Email));

            // Model binding doesn't work for unit testing, so adding in the error manually for testing purposes.
            _eventsController.ModelState.AddModelError("Email", "Invalid Email Address");
            var result = await _eventsController.Create(new Enquiry() { Email = "InvalidEmail", Message = "Another Message" });
            Assert.DoesNotContain("InvalidEmail", _context.Enquiry.Select(e => e.Email));
        }
    }
}
