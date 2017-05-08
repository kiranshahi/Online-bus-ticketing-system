using AutoMapper;
using BusTicketing.Models;
using Entities.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    public class LoadPartialController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;

        public LoadPartialController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // GET: LoadPartial
        public ActionResult LoadSlider()
        {
            var vmSliderList = (from c in _repository.GetNT<Bus>()
                                join t in _repository.GetNT<Travel>() on c.Travel equals t.Id
                                select new SliderViewModel
                                {
                                    TravelAgency = t.Travel_Name,
                                    Facilities = c.Facilities,
                                    BusImage = "/Content/BusImage/" + c.BusImage
                                }).ToList();
            return PartialView("_LoadSlider", vmSliderList);
        }
    }
}