using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABCCC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ABCCC.WebAPI
{
    [Produces("application/json")]
    [Route("api/ABCCC")]
    public class ABCCCController : Controller
    {
        private readonly ABCCCDataContext _context;

        public ABCCCController(ABCCCDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IList<CineplexMovie>> GetCineplexMovie()
        {
            var dayOffset = getDayOffset();
            return await _context.CineplexMovie
                .Include(cm => cm.Movie)
                .Include(cm => cm.Cineplex)
                .OrderBy(cm => getDayOrder(cm.Day, dayOffset))
                .ThenBy(cm => cm.Period)
                .ThenBy(cm => cm.Hour)
                .ToListAsync();
        }

        [HttpGet("{CineplexId}")]
        public async Task<IActionResult> GetCineplexMovie([FromRoute]int CineplexId)
        {
            var dayOffset = getDayOffset();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cineplexMovie = await _context.CineplexMovie.Where(cm => cm.CineplexId == CineplexId)
                .Include(cm => cm.Cineplex)
                .Include(cm => cm.Movie)
                .OrderBy(cm => getDayOrder(cm.Day, dayOffset))
                .ThenBy(cm => cm.Period)
                .ThenBy(cm => cm.Hour)
                .ToListAsync();

            if (cineplexMovie == null)
            {
                return NotFound();
            }

            return Ok(cineplexMovie);
        }

        [HttpGet("{CineplexId}/{Day}/{Hour}/{Period}")]
        public async Task<IActionResult> GetCineplexMovie([FromRoute] int CineplexId,
            CineplexMovie.DayOfWeek Day, int Hour, CineplexMovie.TimePeriod Period)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CineplexMovie cineplexMovie = await _context.CineplexMovie
                .Include(cm => cm.Movie)
                .Include(cm => cm.Cineplex)
                .SingleOrDefaultAsync(cm =>
                    cm.CineplexId == CineplexId &&
                    cm.Day == Day &&
                    cm.Hour == Hour &&
                    cm.Period == Period);

            if (cineplexMovie == null)
            {
                return NotFound();
            }

            return Ok(cineplexMovie);
        }

        [HttpGet]
        [Route("Cineplexes")]
        public async Task<IList<Cineplex>> GetDistinctCineplexes()
        {
            return await _context.CineplexMovie
                .Select(cm => cm.Cineplex)
                .Distinct()
                .ToListAsync();
        }

        [HttpGet("MoviesForCineplex/{CineplexId}")]
        public async Task<IList<Movie>> GetMoviesForCineplex(int CineplexId)
        {
            return await _context.CineplexMovie
                .Where(cm => cm.CineplexId == CineplexId)
                .Select(cm => cm.Movie)
                .Distinct()
                .ToListAsync();
        }

        [HttpGet("Sessions/{CineplexId}/{MovieId}")]
        public async Task<IList<CineplexMovie>> GetSessions(int CineplexId, int MovieId)
        {
            var dayOffset = getDayOffset();
            return await _context.CineplexMovie
                .Where(cm => cm.CineplexId == CineplexId && cm.MovieId == MovieId)
                .OrderBy(cm => getDayOrder(cm.Day, dayOffset))
                .ThenBy(cm => cm.Period)
                .ThenBy(cm => cm.Hour)
                .ToListAsync();
        }

        [HttpGet]
        [Route("Movies")]
        public async Task<IList<Movie>> GetDistinctMovies()
        {
            return await _context.CineplexMovie
                .Select(cm => cm.Movie)
                .Distinct()
                .ToListAsync();
        }

        [HttpPost]
        [Route("Update")]
        public async void PostCineplexMovie([FromBody] CineplexMovie cineplexMovie)
        {
            _context.Update(cineplexMovie);
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        [Route("Transaction")]
        public int PostTransaction([FromBody]Transaction transaction)
        {
            var tracker = _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return tracker.Entity.TransactionId;
        }

        private int getDayOffset()
        {
            DateTime date = DateTime.Now;
            var day = date.DayOfWeek;
            int dayOffset = 0;
            switch (day)
            {
                case DayOfWeek.Monday:
                    dayOffset = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dayOffset = 6;
                    break;
                case DayOfWeek.Wednesday:
                    dayOffset = 5;
                    break;
                case DayOfWeek.Thursday:
                    dayOffset = 4;
                    break;
                case DayOfWeek.Friday:
                    dayOffset = 3;
                    break;
                case DayOfWeek.Saturday:
                    dayOffset = 2;
                    break;
                case DayOfWeek.Sunday:
                    dayOffset = 1;
                    break;
                default:
                    break;
            }
            return dayOffset;
        }

        private int getDayOrder(CineplexMovie.DayOfWeek day, int dayOffset)
        {
            return ((int)day + dayOffset) % 7;
        }
    }
}