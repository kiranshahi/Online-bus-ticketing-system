using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Route
    {
        public int Id { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Through { get; set; }
        public decimal Fair { get; set; }
        public int UserId { get; set; }
    }
}
