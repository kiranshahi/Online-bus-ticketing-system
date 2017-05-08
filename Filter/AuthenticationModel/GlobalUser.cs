using Filters.AuthenticationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Filters.AuthenticationModel
{
    public static class GlobalUser
    {
        //public static AuthoringUser GlobalData { get; set; }

        public static AuthoringUser getGlobalUser()
        {
            AuthoringUser globaluser = new AuthoringUser();
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthentication = DependencyResolver.Current.GetService<FormsAuthenticationFactory>();
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                globaluser = new AuthoringUser(ticket);
            }
            return globaluser;
        }

        private static bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }       
    }
}
