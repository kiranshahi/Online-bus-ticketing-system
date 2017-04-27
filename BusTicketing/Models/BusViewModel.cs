using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusTicketing.Models
{
    public class BusViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Bus No.")]
        public string Bus_No { get; set; }
        [Required]
        [Display(Name = "No. Of Seats")]
        public int NoOfSeats { get; set; }
        [Required]
        [Display(Name = "Travel Agency")]
        public int Travel { get; set; }
        public string TravelName { get; set; }
        public List<System.Web.Mvc.SelectListItem> TravelList { get; set; }
        public string Facilities { get; set; }
        [Display(Name = "Bus Image")]
        public string BusImage { get; set; }
        public HttpPostedFileBase BusImageUpload { get; set; }
        public string Departure { get; set; }
        public decimal Fair { get; set; }
    }

    public class SliderViewModel
    {
        public string TravelAgency { get; set; }
        public string Facilities { get; set; }
        public string BusImage { get; set; }

    }

    public class SearchViewModel
    {
        [Required]
        [Display(Name = "From Address")]
        public string FromAddress { get; set; }
        [Required]
        [Display(Name = "To Address")]
        public string ToAddress { get; set; }
        [Required]
        [Display(Name = "Travel Date")]
        public Nullable<System.DateTime> TravelDate { get; set; }

        public IEnumerable<BusViewModel> BusList { get; set; }

    }

    public class BookSeatViewModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        [Required]
        [Display(Name = "Bus No.")]
        public string Bus_No { get; set; }
        [Required]
        [Display(Name = "No. Of Seats")]
        public int NoOfSeats { get; set; }
        [Required]
        [Display(Name = "Travel Agency")]
        public int Travel { get; set; }
        public string TravelName { get; set; }
        public string Facilities { get; set; }
        [Display(Name = "Bus Image")]
        public string BusImage { get; set; }
        public string Departure { get; set; }
        public decimal Fair { get; set; }
        public string BookedSeat { get; set; }
        public List<System.Web.Mvc.SelectListItem> AvailableSeat { get; set; }
        [Required]
        public IEnumerable<string> SelectedSeat { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime BookedDate { get; set; }

        public string UserName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
    }

}