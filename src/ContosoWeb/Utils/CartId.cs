using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ContosoWeb.Utils
{
    public static class CartId
    {
        // We're using HttpContextBase to allow access to cookies.
        public static string GetCartId(HttpContextBase context)
        {
            var sessionCookie = context.Request.Cookies.Get("Session");
            string cartId = null;

            if (sessionCookie == null)
            {
                //A GUID to hold the cartId. 
                cartId = Guid.NewGuid().ToString();

                // Send cart Id as a cookie to the client.
                context.Response.Cookies.Add(new HttpCookie("Session", cartId));
            }
            else
            {
                cartId = sessionCookie.Value;
            }

            return cartId;
        }
    }
}
