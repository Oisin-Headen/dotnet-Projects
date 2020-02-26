using ABCCC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABCCC.Data;

namespace ABCCC.wwwroot.ViewComponents
{
    [ViewComponent(Name ="Cart")]
    public class CartViewComponent : ViewComponent
    {
        private ABCCCDataContext _context;

        public CartViewComponent(ABCCCDataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartlist = HttpContext.Session.GetCart();
            int seats = 0;
            foreach (var booking in cartlist)
            {
                seats += (booking.AdultSeats + booking.ConcessionSeats);
            }
            return await Task.FromResult<IViewComponentResult>(View(seats));
        }
    }
}
