using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_HealthCare_Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminProfile()
        {
            return View();
        }
        public ActionResult MakeAdmin()
        {
            return View();
        }
        public ActionResult DoctorList()
        {
            return View();
        }
        public ActionResult PatientList()
        {
            return View();
        }
        public ActionResult Ambulance()
        {
            return View();
        }
        public ActionResult Perscription()
        {
            return View();
        }


    }
}