using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ABCCC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ABCCC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionsController : Controller
    {
        private readonly ABCCCDataContext _context;

        public TransactionsController(ABCCCDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Transactions
                .Include(t => t.CreditCard)
                .ToListAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Booking(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _context.Transactions
                    .Where(t => t.TransactionId == Id)
                    .Include(t => t.Bookings)
                        .ThenInclude(b => b.Cineplex)
                    .SingleOrDefaultAsync();
                return View(result);
            }
        }
    }
}