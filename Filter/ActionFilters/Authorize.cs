using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Filters.AuthenticationModel;
using System.Web.Routing;


namespace Filters.ActionFilters
{
    public class BusTicketingAuthorize : AuthorizeAttribute
    {
        public string UserTypes { get; set; }
        public BusTicketingAuthorize(params string[] userTypes)
        {
            this.UserTypes = String.Join(",", userTypes);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                AuthoringUser userInfo = (AuthoringUser)filterContext.HttpContext.User.Identity;
                var userTypes = this.UserTypes.Split(',');
                bool authorized = false;
                foreach (var userType in userTypes)
                {
                    if (Convert.ToInt32(userType) == userInfo.UserType)
                    {
                        authorized = true;
                        break;
                    }
                }
                if (!authorized)
                {
                    filterContext.Result = new RedirectResult("~/Error/Index");
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                { "controller", "Account" },
                { "action", "Login" },
                { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                });
            }

        }

    }
}
