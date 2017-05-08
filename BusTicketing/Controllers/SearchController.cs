using AutoMapper;
using BusTicketing.Models;
using Entities.Models;
using Filters.AuthenticationModel;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    public class SearchController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        public SearchController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchBus(SearchViewModel searchViewModel)
        {
            var vmbusList = (from s in _repository.GetNT<Schedule>()
                             join b in _repository.GetNT<Bus>() on s.Bus_Id equals b.Id
                             join r in _repository.Get<Route>() on s.Route_Id equals r.Id
                             join t in _repository.Get<Travel>() on b.Travel equals t.Id
                             where r.Departure.Contains(searchViewModel.FromAddress) && r.Arrival.Contains(searchViewModel.ToAddress) && s.Date == searchViewModel.TravelDate
                             select new BusViewModel
                             {
                                 Id = b.Id,
                                 Bus_No = b.Bus_No,
                                 NoOfSeats = b.NoOfSeats,
                                 TravelName = t.Travel_Name,
                                 Facilities = b.Facilities,
                                 Departure = s.DepartureTime,
                                 Fair = r.Fair
                             }).ToList();

            searchViewModel.BusList = vmbusList;
            return View("Index", searchViewModel);
        }
    }
}