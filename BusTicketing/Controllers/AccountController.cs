using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using AutoMapper;
using Entities.Models;
using Common.Cryptography;
using Filters.AuthenticationCore;
using Filters.AuthenticationBase;
using Repository.Repository;
using BusTicketing.Models;

namespace BusTicketing.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private IFormsAuthenticationFactory _formsAuthenticationFactory;

        public AccountController(IRepositoryFactory repository, IMapper mapper, IFormsAuthenticationFactory formsAuthenticationFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _formsAuthenticationFactory = formsAuthenticationFactory;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var password = Md5Encryption.Encrypt(model.Password);
                var user = _repository.Queryable<User>().Where(c => c.Username == model.UserName && c.Password == password && c.UserRole != 3).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    _formsAuthenticationFactory.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {
            _formsAuthenticationFactory.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(userViewModel);
                user.Password = Md5Encryption.Encrypt(userViewModel.Password);
                user.UserRole = 3;

                _repository.Insert<User>(user);
                _repository.SaveChanges();

                return View("~/Views/Account/Confirm.cshtml");
            }
            return View(userViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginUser(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var password = Md5Encryption.Encrypt(model.Password);
                var user = _repository.Queryable<User>().Where(c => c.Username == model.UserName && c.Password == password && c.UserRole == 3).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    _formsAuthenticationFactory.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                   
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOffUser()
        {
            _formsAuthenticationFactory.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}