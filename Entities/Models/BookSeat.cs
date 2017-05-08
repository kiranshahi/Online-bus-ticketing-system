using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class BookSeat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime Datetime { get; set; }
        public int ScheduleId { get; set; }
        public string Seats { get; set; }
    }
}
