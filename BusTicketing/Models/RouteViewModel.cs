
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusTicketing.Models
{
    public class RouteViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Departure { get; set; }
        [Required]
        public string Arrival { get; set; }
        public string Through { get; set; }
        [Required]
        public decimal Fair { get; set; }
    }
}