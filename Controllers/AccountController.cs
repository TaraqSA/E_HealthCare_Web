using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_HealthCare_Web.Models;

namespace E_HealthCare_Web.Controllers
{
    public class AccountController : Controller
    {
       

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            HealthDBContext healthDBContext = new HealthDBContext();
            LoginDetails loginDetails = healthDBContext.loginDetails.Single(log => log.UserName == username && log.LoginPassWord == password);

            return View(loginDetails);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

    }
}