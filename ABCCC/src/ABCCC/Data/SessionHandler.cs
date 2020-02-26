using ABCCC.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace ABCCC.Data
{
    public static class SessionHandler
    {
        public static readonly string CART_ID = "Cart";

        public static void SetCart(this ISession session, IList<MovieBooking> value)
        {
            session.SetString(CART_ID, JsonConvert.SerializeObject(value));
        }

        public static IList<MovieBooking> GetCart(this ISession session)
        {
            var value = session.GetString(CART_ID);
            // If the cart has been cleared, value will be the string "null"
            if (value == null || value.Equals("null"))
            {
                var newCart = new List<MovieBooking>();
                SetCart(session, newCart);
                return newCart;
            }
            return JsonConvert.DeserializeObject<IList<MovieBooking>>(value);
        }
    }
}
