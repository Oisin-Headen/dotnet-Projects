using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ABCCC.Models;

namespace ABCCC.Controllers
{
    public class CineplexesController : Controller
    {
        private readonly ABCCCDataContext _context;

        public CineplexesController(ABCCCDataContext context)
        {
            _context = context;
        }

        // GET: Cineplexes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cineplex.ToListAsync());
        }

        // GET: Cineplexes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cineplex = await _context.Cineplex
                .SingleOrDefaultAsync(m => m.CineplexId == id);
            if (cineplex == null)
            {
                return NotFound();
            }

            return View(cineplex);
        }
    }
}
