using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_HealthCare_Web.Controllers
{
   
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound404()
        {
            return View();
        }
    }
}