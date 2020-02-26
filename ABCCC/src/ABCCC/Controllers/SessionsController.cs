using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ABCCC.Models;
using Microsoft.EntityFrameworkCore;
using ABCCC.Data;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ABCCC.Controllers
{
    public class SessionsController : Controller
    {
        public static readonly int MAX_BOOKINGS = 5;
        private static readonly int ADULT_PRICE = 45;
        private static readonly int CONCESSION_PRICE = 20;
        public static readonly string WEB_API_URL = "https://localhost:44384/api/ABCCC/";

        public async Task<IActionResult> Index(int cineplexId = -1, int movieId = -1)
        {
            using (var client = new HttpClient())
            {
                var cineplexMovieJson = await client.GetStringAsync(WEB_API_URL);

                var cineplexMovies = JsonConvert.DeserializeObject<IList<CineplexMovie>>(cineplexMovieJson);
                
                IEnumerable<CineplexMovie> filteredSessions = new List<CineplexMovie>();

                var distinctCineplexesJson = await client.GetStringAsync(WEB_API_URL + "Cineplexes");
                ViewData["Cineplexes"] = JsonConvert.DeserializeObject<IList<Cineplex>>(distinctCineplexesJson);

                var distinctMoviesJson = await client.GetStringAsync(WEB_API_URL + "Movies");
                ViewData["Movies"] = JsonConvert.DeserializeObject<IList<Movie>>(distinctMoviesJson);

                if (cineplexId != -1)
                {
                    if (movieId != -1)
                    {
                        filteredSessions = cineplexMovies.Where(s => s.MovieId == movieId && s.CineplexId == cineplexId);
                    }
                    else
                    {
                        filteredSessions = cineplexMovies.Where(s => s.CineplexId == cineplexId);
                    }
                }
                else if (movieId != -1)
                {
                    filteredSessions = cineplexMovies.Where(s => s.MovieId == movieId);
                }
                // If there are no matching sessions (Because the user has changed the parameters), just displays all sessions.
                if (filteredSessions.Count() == 0)
                {
                    return View(cineplexMovies);
                }

                return View(filteredSessions);
            }
        }

        public async Task<IActionResult> Cart()
        {
            var cartList = HttpContext.Session.GetCart();
            var cart = new List<CartListItemViewModel>();
            int price = 0;
            using (var client = new HttpClient())
            {
                foreach (var item in cartList)
                {
                    string cineplexMovieJson;
                    try
                    {
                        cineplexMovieJson = await client.GetStringAsync(WEB_API_URL +
                        $"{item.CineplexId}/{item.Day}/{item.Hour}/{item.Period}");
                    }
                    catch
                    {
                        return NotFound();
                    }
                    

                    var cineplexMovie = JsonConvert.DeserializeObject<CineplexMovie>(cineplexMovieJson);

                    cart.Add(new CartListItemViewModel()
                    {
                        CineplexMovie = cineplexMovie,
                        AdultSeats = item.AdultSeats,
                        ConcessionSeats = item.ConcessionSeats
                    });
                    price += (item.AdultSeats * ADULT_PRICE) + (item.ConcessionSeats * CONCESSION_PRICE);
                }
                return View(new CartViewModel() { Items = cart, Price = price });
            }
        }

        public async Task<IActionResult> Book(int? cineplexId, CineplexMovie.DayOfWeek? day, int? hour, CineplexMovie.TimePeriod? period)
        {
            if (cineplexId == null || day == null || hour == null || period == null)
            {
                return NotFound();
            }
            using (var client = new HttpClient())
            {
                string cineplexMovieJson;
                try
                {
                    cineplexMovieJson = await client.GetStringAsync(WEB_API_URL +
                            $"{cineplexId}/{day}/{hour}/{period}");
                }
                catch
                {
                    return NotFound();
                }

                var cineplexMovie = JsonConvert.DeserializeObject<CineplexMovie>(cineplexMovieJson);
                    
                if (cineplexMovie == null)
                {
                    return NotFound();
                }
                return View(cineplexMovie);
            }
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        public IActionResult ThankYou(int? transactionId)
        {
            if (transactionId == null)
            {
                return NotFound();
            }
            return View(transactionId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Transaction([Bind("Name, Number, ExpiryMonth, ExpiryYear, CCV")]CCInformation creditCard)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var cart = HttpContext.Session.GetCart();
                    foreach (var item in cart)
                    {
                        string cineplexMovieJson;
                        try
                        {
                            cineplexMovieJson = await client.GetStringAsync(WEB_API_URL +
                                $"{item.CineplexId}/{item.Day}/{item.Hour}/{item.Period}");
                        }
                        catch
                        {
                            return NotFound();
                        }

                        var cineplexMovie = JsonConvert.DeserializeObject<CineplexMovie>(cineplexMovieJson);
                        cineplexMovie.AvailableSeats -= (item.ConcessionSeats + item.AdultSeats);

                        var sessionJson = JsonConvert.SerializeObject(cineplexMovie);
                        await client.PostAsync(WEB_API_URL + "Update", new StringContent(sessionJson, Encoding.UTF8, "application/json"));
                    }

                    var transactionJson = JsonConvert.SerializeObject(new Transaction() { Bookings = cart, CreditCard = creditCard });

                    var transactionResponse = await client.PostAsync(WEB_API_URL + "Transaction", new StringContent(transactionJson, 
                        Encoding.UTF8, "application/json"));
                    HttpContext.Session.SetCart(null);
                    var response = await transactionResponse.Content.ReadAsStringAsync();
                    int transactionId = JsonConvert.DeserializeObject<int>(response);
                    return RedirectToAction("ThankYou", new { transactionId });
                }
            }
            return RedirectToAction("Checkout");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([Bind("CineplexId, Day, Hour, Period, AdultSeats, ConcessionSeats")]MovieBooking item)
        {
            using (var client = new HttpClient())
            {
                string cineplexMovieJson;
                try
                {
                    cineplexMovieJson = await client.GetStringAsync(WEB_API_URL +
                        $"{item.CineplexId}/{item.Day}/{item.Hour}/{item.Period}");
                }
                catch
                {
                    return NotFound();
                }

                var cineplexMovie = JsonConvert.DeserializeObject<CineplexMovie>(cineplexMovieJson);

                // Ensures the specified Session Exists
                if (cineplexMovie == null)
                {
                    return NotFound();
                }
                IList<MovieBooking> cart = HttpContext.Session.GetCart();
                int cartCount = 0;
                foreach (var cartItem in cart)
                {
                    cartCount += (cartItem.ConcessionSeats + cartItem.AdultSeats);
                }
                if (cartCount + item.AdultSeats + item.ConcessionSeats > MAX_BOOKINGS)
                {
                    return RedirectToAction("Cart");
                }
                // If there aren't enough seats left, send the user back to th booking page.
                if (cineplexMovie.AvailableSeats < item.AdultSeats + item.ConcessionSeats)
                {
                    return RedirectToAction("Book", new { item.CineplexId, item.Day, item.Hour, item.Period });
                }
                cart.Add(item);
                HttpContext.Session.SetCart(cart);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int movieId, int cineplexId, CineplexMovie.DayOfWeek day, int hour, CineplexMovie.TimePeriod period)
        {
            var cartList = HttpContext.Session.GetCart();
            var newCartList = new List<MovieBooking>();
            foreach (var item in cartList)
            {
                if (!(item.CineplexId == cineplexId && 
                    item.Day == day &&
                    item.Hour == hour &&
                    item.Period == period))
                {
                    newCartList.Add(item);
                }
            }
            HttpContext.Session.SetCart(newCartList);
            return RedirectToAction("Cart");
        }
    }
}