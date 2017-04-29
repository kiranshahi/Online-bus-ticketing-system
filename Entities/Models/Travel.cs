using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Travel
    {
        public int Id { get; set; }
        public string Travel_Name { get; set; }
        public string Travel_Detail { get; set; }
        public int UserId { get; set; }
    }
}
