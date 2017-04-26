using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusTicketing.Models
{
    public class TravelViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Travel Name")]
        public string Travel_Name { get; set; }
        [Display(Name = "Travel Details")]
        public string Travel_Detail { get; set; }

    }
}