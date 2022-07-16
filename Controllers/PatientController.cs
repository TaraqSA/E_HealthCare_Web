using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_HealthCare_Web.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Appoinment()
        {
            return View();
        }

        public ActionResult PatientProfile()
        {
            return View();
        }
        public ActionResult AvailableDoctors()
        {
            return View();
        }
    }
}