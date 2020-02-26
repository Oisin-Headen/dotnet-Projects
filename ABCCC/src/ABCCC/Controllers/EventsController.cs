using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ABCCC.Models;

namespace ABCCC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ABCCCDataContext _context;

        public EventsController(ABCCCDataContext context)
        {
            _context = context;    
        }

        // GET: Events
        public IActionResult Index()
        {
            return View();
        }

        // GET: ThankYou
        public IActionResult ThankYou()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnquiryId,Email,Message")] Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enquiry);
                await _context.SaveChangesAsync();
                return RedirectToAction("ThankYou");
            }
            return RedirectToAction("Index");
        }
    }
}
