using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ABCCC.Models;
using ABCCC.Data;

namespace ABCCC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ABCCCDataContext _context;

        public MoviesController(ABCCCDataContext context)
        {
            _context = context;    
        }

        // GET: Movies
        public IActionResult Index()
        {
            return View(new MoviesViewModel(_context.Movie, _context.MovieComingSoon));
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.AllMovies.SingleOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}
