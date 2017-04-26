using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && ((Common.GlobalMethods.Enumerators.UserRole)Filters.AuthenticationModel.GlobalUser.getGlobalUser().UserType == Common.GlobalMethods.Enumerators.UserRole.Admin || (Common.GlobalMethods.Enumerators.UserRole)Filters.AuthenticationModel.GlobalUser.getGlobalUser().UserType == Common.GlobalMethods.Enumerators.UserRole.TravelAgency))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}