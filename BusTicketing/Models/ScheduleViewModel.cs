using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusTicketing.Models
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Bus No.")]
        public int Bus_Id { get; set; }
        public string BusName { get; set; }
        public List<System.Web.Mvc.SelectListItem> BusList { get; set; }
        [Required]
        [Display(Name = "Route")]
        public int Route_Id { get; set; }
        public string Route { get; set; }
        public List<System.Web.Mvc.SelectListItem> RouteList { get; set; }
        [Required]
        public Nullable<System.DateTime> Date { get; set; }
        [Required]
        [Display(Name = "Departure Time")]
        public string DepartureTime { get; set; }
    }
}