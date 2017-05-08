using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusTicketing.Models
{
    public class UserViewModel
    {
        public int User_Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "User Type")]
        public int UserRole { get; set; }
        public List<System.Web.Mvc.SelectListItem> UserRoleList { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string First_name { get; set; }
        public string Last_name { get; set; }

        [Required]
        public Nullable<System.DateTime> DOB { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Contact { get; set; }
        [Display(Name = "Profile Image")]
        public string ProfileImage { get; set; }
        [Display(Name = "Profile Image")]
        public HttpPostedFileBase ProfileImageUpload { get; set; }

    }

    public class UserProfileViewModel
    {
        public int User_Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "User Type")]
        public int UserRole { get; set; }
        public List<System.Web.Mvc.SelectListItem> UserRoleList { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string First_name { get; set; }
        public string Last_name { get; set; }

        [Required]
        public Nullable<System.DateTime> DOB { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Contact { get; set; }
        [Display(Name = "Profile Image")]
        public string ProfileImage { get; set; }
        [Display(Name = "Profile Image")]
        public HttpPostedFileBase ProfileImageUpload { get; set; }
    }
}