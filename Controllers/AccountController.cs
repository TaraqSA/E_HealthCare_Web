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
using PasswordHashTool;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace E_HealthCare_Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [ActionName("SignUp")]
        public ActionResult SignUpPost(UserSignUpViewModel userSignUPViewModels)
        {
            if (ModelState.IsValid) //checking if user has given correct values to their respective fields
            {
                if (userSignUPViewModels.Gender == "Male")
                {
                    userSignUPViewModels.Gender = "M";
                }
                else if (userSignUPViewModels.Gender == "Female")
                {
                    userSignUPViewModels.Gender = "F";
                }
                else if (userSignUPViewModels.Gender == "Others")
                {
                    userSignUPViewModels.Gender = "O";
                }
                var HashedPassword = PasswordHashManager.CreateHash(userSignUPViewModels.Password); //creating hash password of password provided by user

                //creating database context using entity framework
                using (var databseContext = new E_HealthCareEntities()) // E_HealthCareEntities is DBContext for the connected database which is connected through Entity framework
                {
                    Patient patient = new Patient();
                    LoginDetail loginDetail = new LoginDetail();

                    patient.p_name = userSignUPViewModels.PatientName;
                    patient.p_gender = userSignUPViewModels.Gender;
                    patient.p_address = userSignUPViewModels.Address;
                    patient.p_dateOfBirth = userSignUPViewModels.DateOfBirth;
                    patient.p_Email = userSignUPViewModels.Email;
                    patient.UserName = userSignUPViewModels.UserName;
                    loginDetail.UserName = userSignUPViewModels.UserName;                    
                    loginDetail.passwordHash = HashedPassword;

                    databseContext.Patients.Add(patient);
                    databseContext.LoginDetails.Add(loginDetail);

                    databseContext.SaveChanges();
                    return RedirectToAction("Login");
                }
                //ModelState.Clear();
                //ViewBag.Message = "Signed Up Successfully!!";
                //TempData["Success_Message"] = "Registered Successfully!!";
                //RedirectToAction("SignUp");

            }
            else
            {
                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("SignUp", userSignUPViewModels);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel userLoginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var isvalidUser = IsValidUser(userLoginViewModel);

                //if user is valid and present in database, redirect to Main page after login 
                if (isvalidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Failure", "Incorrect User Name Or Password!");
                    return View();
                }
            }
            else
            {
                return View(userLoginViewModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string resetCode = Guid.NewGuid().ToString();
                var varifyUrl = "/E_HealthCare_Web/Account/ResetPassword/" + resetCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, varifyUrl);
                using (var context = new E_HealthCareEntities())
                {
                    var getUser = (from s in context.Patients
                                   where s.p_Email == model.EmailAddress
                                   select s).FirstOrDefault();

                    if(getUser != null)
                    {
                        getUser.resetCode = resetCode;
                        //context.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid confirm password not match issue, as we had added a confirm password property
                        context.SaveChanges();

                        var subject = "Password Reset Request";
                        var body = "Hi"+getUser.p_name+", <br/> You recently requested a password reset link to reset the " +
                            "password of your account. click the link below to reset your password."+

                            "<br/> <br/> <a href = '" + link + "'>Reset link</a> <br/><br/>" +
                            "if you did requested a password reset, please ignore this message or reply us to let us know. <br/><br/> Thank you.";

                        SendEmail(getUser.p_Email, body, subject);
                        return RedirectToAction("ForgotPasswordConfirmation");
                    }
                    else
                    {   
                        return RedirectToAction("ForgotPasswordConfirmation");
                    }
                }                
            }
            else
            {
                return View();
            }
        }


        private void SendEmail(string emailAddress, string body, string subject)
        {
            using(MailMessage mm = new MailMessage("salmanchannel212@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;
                
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential networkCredential = new NetworkCredential("salmanchannel212@gmail.com", "jzwsyxrbnncpdoqv");

                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            using (var context = new E_HealthCareEntities())
            {
                var getUserBasedOnResetCode = context.Patients.Where(query => query.resetCode == id).FirstOrDefault();
                if(getUserBasedOnResetCode != null)
                {
                    ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel();
                    resetPasswordViewModel.ResetPasswordCode = id;
                    return View(resetPasswordViewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {               
                using(var context = new E_HealthCareEntities())
                {
                    var getUser = context.Patients.Where(q => q.resetCode == model.ResetPasswordCode).FirstOrDefault();
                    var getUserName = getUser.UserName;                   
                    var getLoginUser = context.LoginDetails.Where(q => q.UserName == getUserName).FirstOrDefault();

                    if(getUser != null)
                    {
                        getLoginUser.passwordHash = PasswordHashManager.CreateHash(model.NewPassword); //added hashed new password
                        getUser.resetCode = ""; //reseting reset code to empty string
                        context.Configuration.ValidateOnSaveEnabled = false; //to avoid validation issue
                        context.SaveChanges();
                        message = "Password Successfully Changed!";
                        return RedirectToAction("Login");
                    }

                }
            }
            else
            {
                message = "Something is invalid";                
            }
            TempData["message"] = message;
            return View(model);
        }


        //function to check if User is valid or not
        public LoginDetail IsValidUser(UserLoginViewModel model)
        {
            using (var dataContext = new E_HealthCareEntities())
            {

                //retreives User object from database by using userName entered in login form 
                var getUserObject = (from LoginUser in dataContext.LoginDetails
                                     where LoginUser.UserName.Equals(model.UserName)
                                     select LoginUser).SingleOrDefault();

                var correctPassword = getUserObject.passwordHash;
                bool compared = false;

                if (correctPassword.Length > 15)
                {
                    var comparingPassword = PasswordHashManager.ValidatePassword(model.Password, correctPassword);
                    compared = comparingPassword;
                }

                //Retireving the user details from DB based on username and password enetered by user.
                LoginDetail user = dataContext.LoginDetails.Where(query => query.UserName.Equals(model.UserName) && (compared || query.passwordHash.Equals(model.Password))).SingleOrDefault();

                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }

        [AllowAnonymous]
        public JsonResult IsEmailAlreadyExist(string Email)
        {
            Patient patient = new Patient();

            using (var context = new E_HealthCareEntities())
            {
                patient = context.Patients.Where(query => query.p_Email.ToLower() == Email.ToLower()).FirstOrDefault();
            }
            bool status;

            if (patient != null)
            {
                status = false;
            }
            else
            {
                status = true;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult IsUserNameAlreadyExist(string UserName)
        {
            LoginDetail loginDetail = new LoginDetail();

            using (var context = new E_HealthCareEntities())
            {
                loginDetail = context.LoginDetails.Where(query => query.UserName.ToLower() == UserName.ToLower()).FirstOrDefault();
            }
            bool status;

            if (loginDetail != null)
            {
                status = false;
            }
            else
            {
                status = true;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }


        public ActionResult DeleteAccount()
        {
            return View();
        }






    }
}