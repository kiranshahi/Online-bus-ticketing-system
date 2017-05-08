using AutoMapper;
using Commom.GlobalMethods;
using Common.Cryptography;
using Common.GlobalMethods;
using Entities.Models;
using Filters.ActionFilters;
using Filters.AuthenticationModel;
using BusTicketing.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    public class UserController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        public UserController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //
        // GET: /User/
        [BusTicketingAuthorize("1")]
        public ActionResult Index()
        {
            var userList = _repository.Get<User>().Where(c=>c.UserRole != 1);
            return View(userList);
        }

        public ActionResult GetUserImage(int id = 0)
        {
            string filepath = "";
            string ImagePath = string.Empty;
            try
            {
                var user = _repository.Get<User>().Where(c => c.User_Id == id).FirstOrDefault();
                if (user != null)
                {
                    ImagePath = "~/Content/ProfileImage/" + user.ProfileImage;
                    filepath = !System.IO.File.Exists(Server.MapPath(ImagePath)) ? Server.MapPath("~/Content/Images/icon-user-default.png")
                                                                   : Server.MapPath(ImagePath);
                }
                else
                {
                    filepath = Server.MapPath("~/Content/Images/icon-user-default.png");
                }
            }
            catch (Exception)
            {
                filepath = Server.MapPath("~/Content/Images/icon-user-default.png");
            }
            return File(filepath, "image/jpg/gif/png");

        }

        [BusTicketingAuthorize("1")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = _repository.Get<User>().Where(c => c.User_Id == id).FirstOrDefault();
                _repository.Delete<User>(user);
                _repository.SaveChanges();
                TempData["Success"] = "User deleted successfully!!";
                TempData["isSuccess"] = "true";

            }
<<<<<<< HEAD
            catch (Exception e)
=======
            catch
>>>>>>> 26ef721075f7daf65910c438cea0051f4b8a7e75
            {
                TempData["Success"] = "Delete Failed!!";
                TempData["isSuccess"] = "false";
            }

            return RedirectToAction("index");
        }
        [BusTicketingAuthorize("1")]
        public ActionResult Create()
        {
            UserViewModel userViewModel = new UserViewModel();
            var userRoleList = Enum.GetNames(typeof(Enumerators.UserRole)).ToList();
            userViewModel.UserRoleList = (from u in userRoleList
                                          select new SelectListItem
                                          {
                                              Value = ((Int32)Enum.Parse(typeof(Enumerators.UserRole), u)).ToString(),
                                              Text = u.ToString()
                                          }).Where(c => c.Value != "1").ToList();
            return View(userViewModel);
        }

        [BusTicketingAuthorize("1")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel)
        {            
            var userRoleList = Enum.GetNames(typeof(Enumerators.UserRole)).ToList();
            userViewModel.UserRoleList = (from u in userRoleList
                                          select new SelectListItem
                                          {
                                              Value = ((Int32)Enum.Parse(typeof(Enumerators.UserRole), u)).ToString(),
                                              Text = u.ToString()
                                          }).Where(c => c.Value != "1").ToList();
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(userViewModel);
                user.Password = Md5Encryption.Encrypt(userViewModel.Password);
                try
                {
                    _repository.Insert<User>(user);
                    _repository.SaveChanges();

                    FileOperations.CreateDirectory(Server.MapPath("~/Content/ProfileImage"));
                    if (userViewModel.ProfileImageUpload != null)
                    {
                        string ext = Path.GetExtension(userViewModel.ProfileImageUpload.FileName).ToLower();
                        string filename = user.User_Id + ext;

                        string filePath = Server.MapPath("~/Content/ProfileImage/") + filename;
                        userViewModel.ProfileImageUpload.SaveAs(filePath);
                        user.ProfileImage = filename;
                    }
                    _repository.Update<User>(user);
                    _repository.SaveChanges();

                    TempData["Success"] = "User created successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Failed to create User!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }
            return View(userViewModel);
        }

        [BusTicketingAuthorize("1")]
        public ActionResult Edit(int id)
        {
            var user = _repository.Get<User>().Where(c => c.User_Id == id).FirstOrDefault();
            var userViewModel = _mapper.Map<UserProfileViewModel>(user);
            var userRoleList = Enum.GetNames(typeof(Enumerators.UserRole)).ToList();
            userViewModel.UserRoleList = (from u in userRoleList
                                          select new SelectListItem
                                          {
                                              Value = ((Int32)Enum.Parse(typeof(Enumerators.UserRole), u)).ToString(),
                                              Text = u.ToString()
                                          }).Where(c => c.Value != "1").ToList();
            return View(userViewModel);
        }

        [BusTicketingAuthorize("1")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(UserProfileViewModel userViewModel)
        {
            var userRoleList = Enum.GetNames(typeof(Enumerators.UserRole)).ToList();
            userViewModel.UserRoleList = (from u in userRoleList
                                          select new SelectListItem
                                          {
                                              Value = ((Int32)Enum.Parse(typeof(Enumerators.UserRole), u)).ToString(),
                                              Text = u.ToString()
                                          }).Where(c => c.Value != "1").ToList();
            if (ModelState.IsValid)
            {
                var user = _repository.Get<User>().Where(c => c.User_Id == userViewModel.User_Id).FirstOrDefault();
                user.First_name = userViewModel.First_name;
                user.Last_name = userViewModel.First_name;
                user.DOB = userViewModel.DOB;
                user.Contact = userViewModel.Contact;
                user.Address = userViewModel.Address;
                user.UserRole = userViewModel.UserRole;
                if (!string.IsNullOrEmpty(userViewModel.Password))
                {
                    user.Password = userViewModel.Password;
                }
                try
                {
                    FileOperations.CreateDirectory(Server.MapPath("~/Content/ProfileImage"));

                    if (userViewModel.ProfileImageUpload != null)
                    {
                        string ext = Path.GetExtension(userViewModel.ProfileImageUpload.FileName).ToLower();
                        string filename = userViewModel.User_Id + ext;

                        string filePath = Server.MapPath("~/Content/ProfileImage/") + filename;
                        userViewModel.ProfileImageUpload.SaveAs(filePath);
                        user.ProfileImage = filename;
                    }
                    _repository.Update<User>(user);
                    _repository.SaveChanges();
                    TempData["Success"] = "User updated successfully!!";
                    TempData["isSuccess"] = "true";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Success"] = "Update Failed!!";
                    TempData["isSuccess"] = "false";
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }

            return View(userViewModel);
        }

        [BusTicketingAuthorize("1", "2", "3")]
        public ActionResult UserProfile()
        {
            var userid = Filters.AuthenticationModel.GlobalUser.getGlobalUser().Id;
            var user = _repository.Get<User>().Where(c => c.User_Id == userid).FirstOrDefault();
            var vmUser = _mapper.Map<UserViewModel>(user);
            return View(vmUser);
        }

        [BusTicketingAuthorize("1", "2", "3")]
        public ActionResult ProfileOverview()
        {
            var userId = GlobalUser.getGlobalUser().Id;
            var user = _repository.Get<User>().Where(c => c.User_Id == userId).FirstOrDefault();
            var vmUser = _mapper.Map<UserViewModel>(user);
            return PartialView("_ProfileOverview", vmUser);
        }

        [BusTicketingAuthorize("1", "2", "3")]
        public ActionResult ProfileEdit()
        {
            var userId = GlobalUser.getGlobalUser().Id;
            var user = _repository.Get<User>().Where(c => c.User_Id == userId).FirstOrDefault();
            var vmUser = _mapper.Map<UserProfileViewModel>(user);
            return PartialView("_ProfileEdit", vmUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProfileEdit(UserProfileViewModel userViewModel)
        {
            string JsonStr = "";
            bool isSuccess = true;
            string message = "Update Successful!!";

            if (ModelState.IsValid)
            {
                var user = _repository.Get<User>().Where(c => c.User_Id == userViewModel.User_Id).FirstOrDefault();
                user.First_name = userViewModel.First_name;
                user.Last_name = userViewModel.Last_name;
                user.DOB = userViewModel.DOB;
                user.Address = userViewModel.Address;
                user.Contact = userViewModel.Contact;
                try
                {
                    FileOperations.CreateDirectory(Server.MapPath("~/Content/ProfileImage"));

                    if (userViewModel.ProfileImageUpload != null)
                    {
                        string ext = Path.GetExtension(userViewModel.ProfileImageUpload.FileName).ToLower();
                        string filename = userViewModel.User_Id + ext;

                        string filePath = Server.MapPath("~/Content/ProfileImage/") + filename;
                        userViewModel.ProfileImageUpload.SaveAs(filePath);
                        user.ProfileImage = filename;
                    }
                    _repository.Update<User>(user);
                    _repository.SaveChanges();
                }
                catch
                {
                    message = "Update Failed!!";
                    isSuccess = false;
                }
            }
            else
            {
                TempData["Success"] = ModelState.Values.SelectMany(m => m.Errors).FirstOrDefault().ErrorMessage;
                TempData["isSuccess"] = "false";
            }
            TempData["Success"] = message;
            TempData["isSuccess"] = isSuccess.ToString();

            JsonStr = "{\"message\":\"" + message + "\",\"isSuccess\":\"" + isSuccess + "\"}";
            return Json(JsonStr, JsonRequestBehavior.AllowGet);
        }

        [BusTicketingAuthorize("1", "2", "3")]
        public ActionResult ChangePassword()
        {
            var userId = GlobalUser.getGlobalUser().Id;
            var changePassword = new ChangePasswordViewModel();
            changePassword.Id = userId;
            return PartialView("_ChangePassword", changePassword);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string JsonStr = "";
            bool isSuccess = true;
            string message = "Password changed successfully!!";
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _repository.Get<User>().Where(c => c.User_Id == changePassword.Id).FirstOrDefault();
                    user.Password = Md5Encryption.Encrypt(changePassword.Password);
                    _repository.Update<User>(user);
                    _repository.SaveChanges();
                }
                catch (Exception)
                {
                    message = "Failed to change password!!";
                    isSuccess = false;
                }
            }

            JsonStr = "{\"message\":\"" + message + "\",\"isSuccess\":\"" + isSuccess + "\"}";
            return Json(JsonStr, JsonRequestBehavior.AllowGet);
        }
    }
}