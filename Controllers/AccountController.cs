using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Security;

namespace E_HealthCare_Web.Controllers
{
    public class AccountController : Controller
    { 
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SignUp")]
        public ActionResult SignUpPost(UserSignUpViewModel userSignUPViewModels)
        {
            if (ModelState.IsValid) //checking if user has given correct values to their respective fields
            {
                //creating database context using entity framework
                using(var databseContext = new E_HealthCareEntities()) // E_HealthCareEntities is DBContext for the connected database which is connected through Entity framework
                {
                    Patient patient = new Patient();
                    LoginDetail loginDetail = new LoginDetail();

                    patient.PatientName = userSignUPViewModels.PatientName;
                    patient.PatientGenderId = userSignUPViewModels.PatientGenderId;
                    patient.PatientAddress = userSignUPViewModels.PatientAddress;
                    patient.PatientAge = userSignUPViewModels.PatientAge;
                    patient.PatientEmail = userSignUPViewModels.PatientEmail;
                    loginDetail.UserName = userSignUPViewModels.UserName;
                    loginDetail.LoginPassWord = userSignUPViewModels.LoginPassWord;

                    databseContext.Patients.Add(patient);
                    databseContext.LoginDetails.Add(loginDetail);
                    
                    databseContext.SaveChanges();
                }

                //ViewBag.Message = "Signed Up Successfully!!";
                TempData["Success_Message"] = "Registered Successfully!!";
                return View("SignUp");
            }
            else
            {
                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("SignUp", userSignUPViewModels);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                var isvalidUser = IsValidUser(userLoginViewModel);

                //if user is valid and present in database, redirect to Main page after login 
                if(isvalidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Failure", "Wrong UserName Or Password!");
                    return View();
                }
            }
            else
            {
                return View(userLoginViewModel);
            }
        }



        //function to check if User is valid or not
        public LoginDetail IsValidUser(UserLoginViewModel model)
        {
            using (var dataContext = new E_HealthCareEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                LoginDetail user = dataContext.LoginDetails.Where(query => query.UserName.Equals(model.UserName) && query.LoginPassWord.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index","Home");
        }

    }
}