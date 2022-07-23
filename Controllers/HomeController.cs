using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_HealthCare_Web.Models;

namespace E_HealthCare_Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {   
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult BloodBank()
        {
            return View();
        }
        public ActionResult Physiotherapy()
        {
            return View();
        }
        public ActionResult Nephrology()
        {
            return View();
        }
        public ActionResult Neurology()
        {
            return View();
        }
        public ActionResult GeneralSurgery()
        {
            return View();
        }
        public ActionResult Endocrinology()
        {
            return View();
        }
        public ActionResult Pathology()
        {
            return View();
        }

        public ActionResult Patholog()
        {
            return View();
        }
        public ActionResult DentalScience()
        {
            return View();
        }

        public ActionResult Dermatology()
        {
            return View();
        }

        public ActionResult Psychology()
        {
            return View();
        }

        public ActionResult Others()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}