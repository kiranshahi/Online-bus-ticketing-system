using AutoMapper;
using BusTicketing.Models;
using Entities.Models;
using Filters.ActionFilters;
using Filters.AuthenticationModel;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers.Admin
{
    [BusTicketingAuthorize("1", "2")]
    public class UserBookingController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private int currentUserId = GlobalUser.getGlobalUser().Id;
        private int userRole = GlobalUser.getGlobalUser().UserType;
        public UserBookingController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // GET: UserBooking
        public ActionResult Index()
        {
            if (userRole == 1)
            {
                var vmbus = (from bs in _repository.GetNT<BookSeat>().ToList()
                             join u in _repository.GetNT<User>().ToList() on bs.UserId equals u.User_Id
                             join s in _repository.GetNT<Schedule>().ToList() on bs.ScheduleId equals s.Id
                             join b in _repository.GetNT<Bus>().ToList() on s.Bus_Id equals b.Id
                             join r in _repository.Get<Route>().ToList() on s.Route_Id equals r.Id
                             join t in _repository.Get<Travel>().ToList() on b.Travel equals t.Id
                             join p in _repository.Get<Payment>().ToList() on bs.Id equals p.BookSeatId
                             select new BookSeatViewModel
                             {
                                 ScheduleId = s.Id,
                                 Bus_No = b.Bus_No,
                                 NoOfSeats = b.NoOfSeats,
                                 TravelName = t.Travel_Name,
                                 Facilities = b.Facilities,
                                 Departure = s.DepartureTime,
                                 Fair = r.Fair,
                                 BusImage = "/Content/BusImage/" + b.BusImage,
                                 BookedSeat = bs.Seats,
                                 AmountPaid = p.PaidAmount,
                                 BookedDate = bs.Datetime,
                                 UserName = u.First_name + "  " + u.Last_name,
                                 Contact = u.Contact,
                                 Address = u.Address
                             });
                return View(vmbus);
            }
            else
            {
                var vmbus = (from bs in _repository.GetNT<BookSeat>().ToList()
                             join u in _repository.GetNT<User>().ToList() on bs.UserId equals u.User_Id
                             join s in _repository.GetNT<Schedule>().ToList() on bs.ScheduleId equals s.Id
                             join b in _repository.GetNT<Bus>().ToList() on s.Bus_Id equals b.Id
                             join r in _repository.Get<Route>().ToList() on s.Route_Id equals r.Id
                             join t in _repository.Get<Travel>().ToList() on b.Travel equals t.Id
                             join p in _repository.Get<Payment>().ToList() on bs.Id equals p.BookSeatId
                             where b.UserId == currentUserId
                             select new BookSeatViewModel
                             {
                                 ScheduleId = s.Id,
                                 Bus_No = b.Bus_No,
                                 NoOfSeats = b.NoOfSeats,
                                 TravelName = t.Travel_Name,
                                 Facilities = b.Facilities,
                                 Departure = s.DepartureTime,
                                 Fair = r.Fair,
                                 BusImage = "/Content/BusImage/" + b.BusImage,
                                 BookedSeat = bs.Seats,
                                 AmountPaid = p.PaidAmount,
                                 BookedDate = bs.Datetime,
                                 UserName = u.First_name + "  " + u.Last_name,
                                 Contact = u.Contact,
                                 Address = u.Address
                             });
                return View(vmbus);
            }
        }
    }
}