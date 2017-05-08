using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Bus
    {
        public int Id { get; set; }
        public string Bus_No { get; set; }
        public int NoOfSeats { get; set; }
        public int Travel { get; set; }
        public string Facilities { get; set; }
        public string BusImage { get; set; }
        public int UserId { get; set; }
    }
}
