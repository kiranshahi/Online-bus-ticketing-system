using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int Bus_Id { get; set; }
        public int Route_Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string DepartureTime { get; set; }
    }
}
