using AutoMapper;
using Commom.GlobalMethods;
using Common.Cryptography;
using Common.GlobalData;
using Common.GlobalMethods;
using Entities.Models;
using Filters.ActionFilters;
using Filters.AuthenticationModel;
using BusTicketing.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    [BusTicketingAuthorize("1", "2")]
    public class RouteController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private int userId = GlobalUser.getGlobalUser().Id;
        private int userRole = GlobalUser.getGlobalUser().UserType;
        public RouteController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //
        // GET: /User/        
        public ActionResult Index()
        {
            if (userRole == 1)
            {
                var routeList = _repository.Get<Route>().ToList();
                var vmbusList = (from b in routeList
                                 select new RouteViewModel
                                 {
                                     Id = b.Id,
                                     Arrival = b.Arrival,
                                     Departure = b.Departure,
                                     Through = b.Through,
                                     Fair = b.Fair
                                 }).ToList();
                return View(vmbusList);
            }
            else
            {
                var routeList = _repository.Get<Route>().Where(c => c.UserId == userId).ToList();
                var vmbusList = (from b in routeList
                                 select new RouteViewModel
                                 {
                                     Id = b.Id,
                                     Arrival = b.Arrival,
                                     Departure = b.Departure,
                                     Through = b.Through,
                                     Fair = b.Fair
                                 }).ToList();
                return View(vmbusList);
            }
        }       
      
        public ActionResult Delete(int id)
        {
            try
            {
                var route = _repository.Get<Route>().Where(c => c.Id == id && c.UserId == userId).FirstOrDefault();
                _repository.Delete<Route>(route);
                _repository.SaveChanges();
                TempData["Success"] = "Route deleted successfully!!";
                TempData["isSuccess"] = "true";

            }
            catch
            {
                TempData["Success"] = "Delete Failed!!";
                TempData["isSuccess"] = "false";
            }

            return RedirectToAction("index");
        }

        public ActionResult Create()
        {
            RouteViewModel routeViewModel = new RouteViewModel();
            return View(routeViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(RouteViewModel routeViewModel)
        {
            if (ModelState.IsValid)
            {
                var route = _mapper.Map<Route>(routeViewModel);
                try
                {
                    route.UserId = GlobalUser.getGlobalUser().Id;
                    _repository.Insert<Route>(route);
                    _repository.SaveChanges();

                    TempData["Success"] = "Route created successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Failed to create Route!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }
            return View(routeViewModel);
        }

        public ActionResult Edit(int id)
        {
            if (userRole == 1)
            {
                var route = _repository.Get<Route>().Where(c => c.Id == id).FirstOrDefault();
                var routeViewModel = _mapper.Map<RouteViewModel>(route);
                return View(routeViewModel);
            }
            else
            {
                var route = _repository.Get<Route>().Where(c => c.Id == id && c.UserId == userId).FirstOrDefault();
                var routeViewModel = _mapper.Map<RouteViewModel>(route);
                return View(routeViewModel);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(RouteViewModel routeViewModel)
        {
            if (ModelState.IsValid)
            {
                var route = _repository.Get<Route>().Where(c => c.Id == routeViewModel.Id).FirstOrDefault();
                route.Arrival = routeViewModel.Arrival;
                route.Departure = routeViewModel.Departure;
                route.Through = routeViewModel.Through;
                route.Fair = routeViewModel.Fair;
                try
                {                   
                    _repository.Update<Route>(route);
                    _repository.SaveChanges();
                    TempData["Success"] = "Route updated successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Update Failed!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }

            return View(routeViewModel);
        }
    }
}