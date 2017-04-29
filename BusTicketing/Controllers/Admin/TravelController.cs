using AutoMapper;
using Commom.GlobalMethods;
using Common.Cryptography;
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
    public class TravelController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private int userId = GlobalUser.getGlobalUser().Id;
        public TravelController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //
        // GET: /User/        
        public ActionResult Index()
        {
            var userRole = GlobalUser.getGlobalUser().UserType;
            if (userRole == 1)
            {
                var travelList = _repository.Get<Travel>();
                return View(travelList);
            }
            else
            {
                var travelList = _repository.Get<Travel>().Where(c => c.UserId == userId);
                return View(travelList);
            }
        }
       
        public ActionResult Delete(int id)
        {
            try
            {
                var travel = _repository.Get<Travel>().Where(c => c.Id == id).FirstOrDefault();
                _repository.Delete<Travel>(travel);
                _repository.SaveChanges();
                TempData["Success"] = "Travel deleted successfully!!";
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
            TravelViewModel travelViewModel = new TravelViewModel();
            return View(travelViewModel);
        }
       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(TravelViewModel travelViewModel)
        {            
            if (ModelState.IsValid)
            {
                var travel = _mapper.Map<Travel>(travelViewModel);
                try
                {
                    _repository.Insert<Travel>(travel);
                    _repository.SaveChanges();

                    TempData["Success"] = "Travel created successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Failed to create Travel!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }
            return View(travelViewModel);
        }
       
        public ActionResult Edit(int id)
        {
            var travel = _repository.Get<Travel>().Where(c => c.Id == id && c.UserId == userId).FirstOrDefault();
            var travelViewModel = _mapper.Map<TravelViewModel>(travel);
            return View(travelViewModel);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(TravelViewModel travelViewModel)
        {            
            if (ModelState.IsValid)
            {
                var travel = _repository.Get<Travel>().Where(c => c.Id == travelViewModel.Id).FirstOrDefault();
                travel.Travel_Name = travelViewModel.Travel_Name;
                travel.Travel_Detail = travelViewModel.Travel_Detail;
                try
                {
                    _repository.Update<Travel>(travel);
                    _repository.SaveChanges();
                    TempData["Success"] = "Travel updated successfully!!";
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

            return View(travelViewModel);
        }
    }
}