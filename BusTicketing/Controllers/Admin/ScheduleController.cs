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
    public class ScheduleController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private int currentUserId = GlobalUser.getGlobalUser().Id;
        private int userRole = GlobalUser.getGlobalUser().UserType;
        public ScheduleController(IRepositoryFactory repository, IMapper mapper)
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
                var busList = _repository.Get<Schedule>().ToList();
                var vmbusList = (from b in busList
                                 select new ScheduleViewModel
                                 {
                                     Id = b.Id,
                                     BusName = this.GetBusNumber(b.Bus_Id),
                                     Route = this.GetRoute(b.Route_Id),
                                     DepartureTime = b.DepartureTime,
                                     Date = b.Date
                                 }).ToList();
                return View(vmbusList);
            }
            else
            {
                var busList = _repository.Get<Schedule>().ToList();
                var vmbusList = (from b in busList
                                 join bs in _repository.GetNT<Bus>().ToList() on b.Bus_Id equals bs.Id
                                 where bs.UserId == currentUserId
                                 select new ScheduleViewModel
                                 {
                                     Id = b.Id,
                                     BusName = this.GetBusNumber(b.Bus_Id),
                                     Route = this.GetRoute(b.Route_Id),
                                     DepartureTime = b.DepartureTime,
                                     Date = b.Date
                                 }).ToList();
                return View(vmbusList);
            }
        }       

        public string GetBusNumber(int Id)
        {
            string result = string.Empty;
            var bus = _repository.GetNT<Bus>().Where(c => c.Id == Id).FirstOrDefault();
            if (bus != null)
            {
                result = bus.Bus_No;
            }
            return result;
        }

        public string GetRoute(int Id)
        {
            string result = string.Empty;
            var route = _repository.GetNT<Route>().Where(c => c.Id == Id).FirstOrDefault();
            if (route != null)
            {
                result = route.Departure + "-" + route.Arrival;
            }
            return result;
        }    

      
        public ActionResult Delete(int id)
        {
            try
            {
                var schedule = _repository.Get<Schedule>().Where(c => c.Id == id).FirstOrDefault();
                _repository.Delete<Schedule>(schedule);
                _repository.SaveChanges();
                TempData["Success"] = "Schedule deleted successfully!!";
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
            if (userRole == 1)
            {
                ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
                scheduleViewModel.BusList = (from p in _repository.Get<Bus>()
                                             select new SelectListItem
                                             {
                                                 Value = p.Id.ToString(),
                                                 Text = p.Bus_No
                                             }).ToList();
                scheduleViewModel.RouteList = (from p in _repository.Get<Route>()
                                               select new SelectListItem
                                               {
                                                   Value = p.Id.ToString(),
                                                   Text = p.Departure + "-" + p.Arrival
                                               }).ToList();
                return View(scheduleViewModel);
            }
            else
            {
                ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
                scheduleViewModel.BusList = (from p in _repository.Get<Bus>().Where(c => c.UserId == currentUserId)
                                             select new SelectListItem
                                             {
                                                 Value = p.Id.ToString(),
                                                 Text = p.Bus_No
                                             }).ToList();
                scheduleViewModel.RouteList = (from p in _repository.Get<Route>().Where(c => c.UserId == currentUserId)
                                               select new SelectListItem
                                               {
                                                   Value = p.Id.ToString(),
                                                   Text = p.Departure + "-" + p.Arrival
                                               }).ToList();
                return View(scheduleViewModel);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(ScheduleViewModel scheduleViewModel)
        {
            scheduleViewModel.BusList = (from p in _repository.Get<Bus>()
                                         select new SelectListItem
                                         {
                                             Value = p.Id.ToString(),
                                             Text = p.Bus_No
                                         }).ToList();
            scheduleViewModel.RouteList = (from p in _repository.Get<Route>()
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Departure + "-" + p.Arrival
                                           }).ToList();
            if (ModelState.IsValid)
            {
                var schedule = _mapper.Map<Schedule>(scheduleViewModel);
                try
                {                    
                    _repository.Insert<Schedule>(schedule);
                    _repository.SaveChanges();

                    

                    TempData["Success"] = "Schedule created successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Failed to create Schedule!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }
            return View(scheduleViewModel);
        }

        public ActionResult Edit(int id)
        {
            var schedule = _repository.Get<Schedule>().Where(c => c.Id == id).FirstOrDefault();
            var scheduleViewModel = _mapper.Map<ScheduleViewModel>(schedule);
            scheduleViewModel.BusList = (from p in _repository.Get<Bus>()
                                         select new SelectListItem
                                         {
                                             Value = p.Id.ToString(),
                                             Text = p.Bus_No
                                         }).ToList();
            scheduleViewModel.RouteList = (from p in _repository.Get<Route>()
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Departure + "-" + p.Arrival
                                           }).ToList();
            return View(scheduleViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(ScheduleViewModel scheduleViewModel)
        {
            scheduleViewModel.BusList = (from p in _repository.Get<Bus>()
                                         select new SelectListItem
                                         {
                                             Value = p.Id.ToString(),
                                             Text = p.Bus_No
                                         }).ToList();
            scheduleViewModel.RouteList = (from p in _repository.Get<Route>()
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Departure + "-" + p.Arrival
                                           }).ToList();
            if (ModelState.IsValid)
            {
                var schedule = _repository.Get<Schedule>().Where(c => c.Id == scheduleViewModel.Id).FirstOrDefault();
                schedule.Bus_Id = scheduleViewModel.Bus_Id;
                schedule.Route_Id = scheduleViewModel.Route_Id;
                schedule.Date = scheduleViewModel.Date;
                schedule.DepartureTime = scheduleViewModel.DepartureTime;
                try
                {                    
                    _repository.Update<Schedule>(schedule);
                    _repository.SaveChanges();
                    TempData["Success"] = "Schedule updated successfully!!";
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

            return View(scheduleViewModel);
        }

       
        
    }
}