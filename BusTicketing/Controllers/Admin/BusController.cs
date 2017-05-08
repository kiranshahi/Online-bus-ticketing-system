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
    public class BusController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private int userId = GlobalUser.getGlobalUser().Id;
        private int userRole = GlobalUser.getGlobalUser().UserType;
        public BusController(IRepositoryFactory repository, IMapper mapper)
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
                var busList = _repository.Get<Bus>().ToList();
                var vmbusList = (from b in busList
                                 select new BusViewModel
                                 {
                                     Id = b.Id,
                                     Bus_No = b.Bus_No,
                                     NoOfSeats = b.NoOfSeats,
                                     TravelName = this.GetTravelName(b.Travel),
                                     Facilities= b.Facilities
                                 }).ToList();
                return View(vmbusList);
            }
            else
            {
                var busList = _repository.Get<Bus>().Where(c => c.UserId == userId).ToList();
                var vmbusList = (from b in busList
                                 select new BusViewModel
                                 {
                                     Id = b.Id,
                                     Bus_No = b.Bus_No,
                                     NoOfSeats = b.NoOfSeats,
                                     TravelName = this.GetTravelName(b.Travel),
                                     Facilities = b.Facilities
                                 }).ToList();
                return View(vmbusList);
            }
        }

        public ActionResult GetBusImage(int id = 0)
        {
            string filepath = "";
            string ImagePath = string.Empty;
            try
            {
                var bus = _repository.Get<Bus>().Where(c => c.Id == id).FirstOrDefault();
                if (bus != null)
                {
                    ImagePath = "~/Content/BusImage/" + bus.BusImage;
                    filepath = !System.IO.File.Exists(Server.MapPath(ImagePath)) ? Server.MapPath("~/Content/Images/icon-user-default.png")
                                                                   : Server.MapPath(ImagePath);
                }
                else
                {
                    filepath = Server.MapPath("~/Content/Images/icon-user-default.png");
                }
            }
            catch (Exception)
            {
                filepath = Server.MapPath("~/Content/Images/icon-user-default.png");
            }
            return File(filepath, "image/jpg/gif/png");

        }

        public string GetTravelName(int Id)
        {
            string result = string.Empty;
            var travel = _repository.GetNT<Travel>().Where(c => c.Id == Id).FirstOrDefault();
            if (travel != null)
            {
                result = travel.Travel_Name;
            }
            return result;
        }       

      
        public ActionResult Delete(int id)
        {
            try
            {
                var bus = _repository.Get<Bus>().Where(c => c.Id == id).FirstOrDefault();
                _repository.Delete<Bus>(bus);
                _repository.SaveChanges();
                TempData["Success"] = "Bus deleted successfully!!";
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
                BusViewModel busViewModel = new BusViewModel();
                busViewModel.TravelList = (from p in _repository.Get<Travel>()
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Travel_Name
                                           }).ToList();
                return View(busViewModel);
            }
            else
            {
                BusViewModel busViewModel = new BusViewModel();
                busViewModel.TravelList = (from p in _repository.Get<Travel>().Where(c => c.UserId == userId)
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Travel_Name
                                           }).ToList();
                return View(busViewModel);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(BusViewModel busViewModel)
        {
            busViewModel.TravelList = (from p in _repository.Get<Travel>()
                                       select new SelectListItem
                                       {
                                           Value = p.Id.ToString(),
                                           Text = p.Travel_Name
                                       }).ToList();
            if (ModelState.IsValid)
            {
                var bus = _mapper.Map<Bus>(busViewModel);
                try
                {
                    bus.UserId = GlobalUser.getGlobalUser().Id;
                    _repository.Insert<Bus>(bus);
                    _repository.SaveChanges();

                    FileOperations.CreateDirectory(Server.MapPath("~/Content/BusImage"));
                    if (busViewModel.BusImageUpload != null)
                    {
                        string ext = Path.GetExtension(busViewModel.BusImageUpload.FileName).ToLower();
                        string filename = bus.Id + ext;

                        string filePath = Server.MapPath("~/Content/BusImage/") + filename;
                        busViewModel.BusImageUpload.SaveAs(filePath);
                        bus.BusImage = filename;
                    }
                    _repository.Update<Bus>(bus);
                    _repository.SaveChanges();

                    TempData["Success"] = "Bus created successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Failed to create Bus!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }
            return View(busViewModel);
        }

        public ActionResult Edit(int id)
        {
            if (userRole == 1)
            {
                var bus = _repository.Get<Bus>().Where(c => c.Id == id).FirstOrDefault();
                var busViewModel = _mapper.Map<BusViewModel>(bus);
                busViewModel.TravelList = (from p in _repository.Get<Travel>()
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Travel_Name
                                           }).ToList();
                return View(busViewModel);
            }
            else
            {
                var bus = _repository.Get<Bus>().Where(c => c.Id == id).FirstOrDefault();
                var busViewModel = _mapper.Map<BusViewModel>(bus);
                busViewModel.TravelList = (from p in _repository.Get<Travel>().Where(c => c.UserId == userId)
                                           select new SelectListItem
                                           {
                                               Value = p.Id.ToString(),
                                               Text = p.Travel_Name
                                           }).ToList();
                return View(busViewModel);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(BusViewModel busViewModel)
        {
            busViewModel.TravelList = (from p in _repository.Get<Travel>()
                                       select new SelectListItem
                                       {
                                           Value = p.Id.ToString(),
                                           Text = p.Travel_Name
                                       }).ToList();
            if (ModelState.IsValid)
            {
                var bus = _repository.Get<Bus>().Where(c => c.Id == busViewModel.Id).FirstOrDefault();
                bus.Bus_No = busViewModel.Bus_No;
                bus.NoOfSeats = busViewModel.NoOfSeats;
                bus.Travel = busViewModel.Travel;
                bus.Facilities = busViewModel.Facilities;
                try
                {
                    FileOperations.CreateDirectory(Server.MapPath("~/Content/BusImage"));
                    if (busViewModel.BusImageUpload != null)
                    {
                        string ext = Path.GetExtension(busViewModel.BusImageUpload.FileName).ToLower();
                        string filename = bus.Id + ext;

                        string filePath = Server.MapPath("~/Content/BusImage/") + filename;
                        busViewModel.BusImageUpload.SaveAs(filePath);
                        bus.BusImage = filename;
                    }
                    _repository.Update<Bus>(bus);
                    _repository.SaveChanges();
                    TempData["Success"] = "Bus updated successfully!!";
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

            return View(busViewModel);
        }

       
        
    }
}