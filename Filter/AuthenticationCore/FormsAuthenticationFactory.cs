using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filters.AuthenticationBase;
using Filters.AuthenticationModel;
using System.Web.Security;
using System.Web;

namespace Filters.AuthenticationCore
{
    public class FormsAuthenticationFactory
        : IFormsAuthenticationFactory
    {
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void SetAuthCookie(string userName , bool persistent)
        {
            FormsAuthentication.SetAuthCookie(userName, persistent);
        }

        public static DateTime CalculateCookieExpirationDate()
        {
            return DateTime.Now.Add(FormsAuthentication.Timeout);
        }

        public FormsAuthenticationTicket Decrypt(string encryptedTicket)
        {
            return FormsAuthentication.Decrypt(encryptedTicket);
        }

        public void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket)
        {
            var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
            httpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Expires = CalculateCookieExpirationDate() });
        }

        public void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket)
        {
            var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
            httpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Expires = CalculateCookieExpirationDate() });
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var cticket = FormsAuthentication.Decrypt(authCookie.Value);
        }
    }
}
