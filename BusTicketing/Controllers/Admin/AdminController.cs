using AutoMapper;
using Filters.AuthenticationBase;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketing.Controllers
{
    public class AdminController : Controller
    {
        private IRepositoryFactory _repository;
        private IMapper _mapper;
        private IFormsAuthenticationFactory _formsAuthenticationFactory;

        public AdminController(IRepositoryFactory repository, IMapper mapper, IFormsAuthenticationFactory formsAuthenticationFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _formsAuthenticationFactory = formsAuthenticationFactory;
        }
        // GET: Admin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                _formsAuthenticationFactory.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}