using AutoMapper;
using BusTicketing.Models;
using Entities.Models;
using Filters.ActionFilters;
using Filters.AuthenticationModel;
using MultiSafepay;
using MultiSafepay.Model;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    [BusTicketingAuthorize("3")]
    public class BookController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        public BookController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public ActionResult Book(int id)
        {
            var vmbus = (from s in _repository.GetNT<Schedule>().ToList()
                         join b in _repository.GetNT<Bus>().ToList() on s.Bus_Id equals b.Id
                         join r in _repository.Get<Route>().ToList() on s.Route_Id equals r.Id
                         join t in _repository.Get<Travel>().ToList() on b.Travel equals t.Id
                         where b.Id == id
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
                             BookedSeat = GetBookedSeat(s.Id)
                         }).FirstOrDefault();
            vmbus.AvailableSeat = GetAvailableSeat(vmbus.BookedSeat, vmbus.NoOfSeats);
            return View(vmbus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(BookSeatViewModel vmbookseat)
        {
            vmbookseat.AvailableSeat = GetAvailableSeat(vmbookseat.BookedSeat, vmbookseat.NoOfSeats);
            if (ModelState.IsValid)
            {
                var bookseat = new BookSeat();
                bookseat.ScheduleId = vmbookseat.ScheduleId;
                bookseat.UserId = GlobalUser.getGlobalUser().Id;
                bookseat.Seats = string.Join(",", vmbookseat.SelectedSeat);// vmbookseat.SelectedSeat.Join(",");
                bookseat.Datetime = DateTime.Now;
                _repository.Insert<BookSeat>(bookseat);
                _repository.SaveChanges();

                var payment = new Payment();
                payment.PaymentId = Guid.NewGuid().ToString();
                payment.User_Id = GlobalUser.getGlobalUser().Id;
                payment.BookSeatId = bookseat.Id;
                payment.PaidAmount = vmbookseat.SelectedSeat.Count() * vmbookseat.Fair;
                _repository.Insert<Payment>(payment);
                _repository.SaveChanges();

                var url = ConfigurationManager.AppSettings["MultiSafepayAPI"];
                var apiKey = ConfigurationManager.AppSettings["MultiSafepayAPIKey"];
                var client = new MultiSafepayClient(apiKey, url);
                var orderRequest = OrderRequest.CreateRedirect(payment.PaymentId, bookseat.Seats, Convert.ToInt32(payment.PaidAmount), "EUR", new PaymentOptions("http://localhost:1297/Book/Notify", "http://localhost:1297/Book/Success", "http://localhost:1297/Book/Failed"));

                var result = client.CreateOrder(orderRequest);
                return Redirect(result.PaymentUrl.ToString());
            }
            return View(vmbookseat);
        }

        public string GetBookedSeat(int id)
        {
            var bookedseat = _repository.GetNT<BookSeat>().Where(c => c.ScheduleId == id);
            var result = string.Empty;
            foreach (var bs in bookedseat)
            {
                var seats = bs.Seats.Split(',');
                foreach (var seat in seats)
                {
                    if (!string.IsNullOrEmpty(seat))
                    {
                        result = result + "," + seat;
                    }
                }
            }

            return string.IsNullOrEmpty(result) ? "" : result.Substring(1);
        }

        public List<System.Web.Mvc.SelectListItem> GetAvailableSeat(string bookedseat, int seats)
        {
            List<System.Web.Mvc.SelectListItem> result = new List<SelectListItem>();
            var bookedseatarray = (bookedseat ?? "").Split(',').ToList();
            for (int i = 1; i <= seats; i++)
            {
                if (bookedseatarray.IndexOf(i.ToString()) == -1)
                {
                    result.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
            }
            return result;
        }

        public ActionResult MyBooking()
        {
            var userId = GlobalUser.getGlobalUser().Id;
            var vmbus = (from bs in _repository.GetNT<BookSeat>().ToList()
                         join s in _repository.GetNT<Schedule>().ToList() on bs.ScheduleId equals s.Id
                         join b in _repository.GetNT<Bus>().ToList() on s.Bus_Id equals b.Id
                         join r in _repository.Get<Route>().ToList() on s.Route_Id equals r.Id
                         join t in _repository.Get<Travel>().ToList() on b.Travel equals t.Id
                         join p in _repository.Get<Payment>().ToList() on bs.Id equals p.BookSeatId
                         where bs.UserId == userId
                         select new BookSeatViewModel
                         {
                             Id = bs.Id,
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
                             BookedDate = bs.Datetime
                         });
            return View(vmbus);
        }

        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Failed()
        {
            return View();
        }
        public ActionResult Cancel(int id)
        {
            try
            {
                var bookSeat = _repository.Get<BookSeat>().Where(c => c.Id == id).FirstOrDefault();
                _repository.Delete<BookSeat>(bookSeat);
                _repository.SaveChanges();
                TempData["Success"] = "Ticket has been cancelled successfully!!";
                TempData["isSuccess"] = "true";

            }
            catch
            {
                TempData["Success"] = "Cancel Failed!!";
                TempData["isSuccess"] = "false";
            }

            return RedirectToAction("MyBooking");
        }
    }
}