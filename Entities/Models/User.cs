using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class User
    {
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserRole { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ProfileImage { get; set; }
    }
}
