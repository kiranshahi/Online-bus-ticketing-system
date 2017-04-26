using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Filters.AuthenticationBase
{
    public interface IFormsAuthenticationFactory
    {
        void SignOut();
        void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket);
        void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket);
        FormsAuthenticationTicket Decrypt(string encryptedTicket);
    }
}
