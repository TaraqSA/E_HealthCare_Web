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
        [Authorize]
        public ActionResult SignUpPost(UserSignUpViewModel userSignUPViewModels)
        {
            if (ModelState.IsValid) //checking if user has given correct values to their respective fields
            {
                var HashedPassword = PasswordHashManager.CreateHash(userSignUPViewModels.Password); //creating hash password of password provided by user

                //creating database context using entity framework
                using (var databseContext = new E_HealthCareEntities()) // E_HealthCareEntities is DBContext for the connected database which is connected through Entity framework
                {
                    if (userSignUPViewModels.UserRole == "Patient")
                    {

                        Patient patient = new Patient();
                        SiteUser siteUser = new SiteUser();

                        siteUser.Email = userSignUPViewModels.Email;
                        siteUser.UserName = userSignUPViewModels.UserName;
                        siteUser.PasswordHash = HashedPassword;
                        siteUser.UserRole = "Patient";
                        patient.p_Email = userSignUPViewModels.Email;
                        patient.UserName = userSignUPViewModels.UserName;
                        patient.SiteUser = siteUser;

                        siteUser.Patients.Add(patient);//for better understanding visit https://docs.microsoft.com/en-us/ef/ef6/fundamentals/relationships  
                        databseContext.SiteUsers.Add(siteUser);
                        //databseContext.Patients.Add(patient);
                        databseContext.SaveChanges();

                        return RedirectToAction("Login");
                    }
                    else if (userSignUPViewModels.UserRole == "Doctor")
                    {
                        Doctor doctor = new Doctor();
                        SiteUser siteUser = new SiteUser();

                        siteUser.Email = userSignUPViewModels.Email;
                        siteUser.UserName = userSignUPViewModels.UserName;
                        siteUser.PasswordHash = HashedPassword;
                        siteUser.UserRole = "Doctor";
                        doctor.D_Email = userSignUPViewModels.Email;
                        doctor.D_UserName = userSignUPViewModels.UserName;
                        doctor.SiteUser = siteUser;

                        databseContext.SiteUsers.Add(siteUser);
                        databseContext.Doctors.Add(doctor);
                        databseContext.SaveChanges();

                        return RedirectToAction("Login");

                    }
                    return View("SignUp", userSignUPViewModels);
                }
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
                var context = new E_HealthCareEntities();
                var isvalidUser = IsValidUser(userLoginViewModel);


                //if user is valid and present in database, redirect to Main page after login 
                if (isvalidUser != null)
                {
                    var getRole = context.SiteUsers.Where(q => q.UserName == userLoginViewModel.UserName).FirstOrDefault().UserRole;
                    if (getRole == "Patient")
                    {
                        FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                        return RedirectToAction("Index", "Patient");
                    }
                    else if (getRole == "Doctor")
                    {
                        FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                        return RedirectToAction("Index", "Doctor");
                    }
                    else if (getRole == "Admin")
                    {
                        FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                        return RedirectToAction("Index", "Admin");
                    }
                    return View(userLoginViewModel);

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
                    var getUser = (from s in context.SiteUsers
                                   where s.Email == model.EmailAddress
                                   select s).FirstOrDefault();
                    //var getUsers = context.Patients.Where(p => p.p_Email == model.EmailAddress).FirstOrDefault();

                    if (getUser != null)
                    {
                        getUser.PasswordResetCode = resetCode;

                        //context.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid confirm password not match issue, as we had added a confirm password property
                        context.SaveChanges();

                        var subject = "Password Reset Request";
                        var body = "Hi " + getUser.UserName + ", <br/> You recently requested a password reset link to reset the " +
                            "password of your account. click the link below to reset your password." +

                            "<br/> <br/> <a href = '" + link + "'>Reset link</a> <br/><br/>" +
                            "if you did requested a password reset, please ignore this message or reply us to let us know. <br/><br/> Thank you.";

                        SendEmail(getUser.Email, body, subject);

                        UrlLoggin urlLoggin = new UrlLoggin();//adding url data to table to check if url is expired or not
                        urlLoggin.User_Id = getUser.Id;
                        urlLoggin.Create_Date = DateTime.Now;
                        urlLoggin.Link_Status = true;
                        urlLoggin.Reset_Url_Path = resetCode;
                        context.UrlLoggins.Add(urlLoggin);
                        context.SaveChanges();
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

        [AllowAnonymous]
        public ActionResult ExpiredLink()
        {
            return View();
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("salmanchannel212@gmail.com", emailAddress))
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

            var context = new E_HealthCareEntities();
            var getUrlDetail = context.UrlLoggins.Where(q => q.Reset_Url_Path == id).FirstOrDefault();
            if (!(getUrlDetail == null))
            {
                var dateDifference = DateTime.Now.Subtract(getUrlDetail.Create_Date.Value);
                if (dateDifference.Days < 1)
                {
                    using (context)
                    {
                        var getUserBasedOnResetCode = context.SiteUsers.Where(query => query.PasswordResetCode == id).FirstOrDefault();
                        if (getUserBasedOnResetCode != null)
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
                else
                {
                    getUrlDetail.Link_Status = false;
                    context.SaveChanges();
                    return RedirectToAction("ExpiredLink");
                }                
            }
            return RedirectToAction("ExpiredLink");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new E_HealthCareEntities())
                {
                    var getUser = context.SiteUsers.Where(q => q.PasswordResetCode == model.ResetPasswordCode).FirstOrDefault();
                    var getUserName = getUser.UserName;
                    var getLoginUser = context.SiteUsers.Where(q => q.UserName == getUserName).FirstOrDefault();

                    if (getUser != null)
                    {
                        getLoginUser.PasswordHash = PasswordHashManager.CreateHash(model.NewPassword); //added hashed new password
                        getUser.PasswordResetCode = ""; //reseting reset code to empty string
                        context.Configuration.ValidateOnSaveEnabled = false; //to avoid validation issue
                        context.SaveChanges();
                        message = "Password Successfully Changed!";
                        return RedirectToAction("Login");
                    }
                }
            }
            else
            {
                message = "Something went wrong";
            }
            TempData["message"] = message;
            return View(model);
        }


        //function to check if User is valid or not
        public SiteUser IsValidUser(UserLoginViewModel model)
        {
            using (var dataContext = new E_HealthCareEntities())
            {

                //retreives User object from database by using userName entered in login form 
                var getUserObject = (from LoginUser in dataContext.SiteUsers
                                     where LoginUser.UserName.Equals(model.UserName)
                                     select LoginUser).SingleOrDefault();
                if (getUserObject == null)
                {
                    return null;
                }

                var correctPassword = getUserObject.PasswordHash;
                bool compared = false;

                if (correctPassword.Length > 15)
                {
                    var comparingPassword = PasswordHashManager.ValidatePassword(model.Password, correctPassword);
                    compared = comparingPassword;
                }

                //Retireving the user details from DB based on username and password enetered by user.
                SiteUser user = dataContext.SiteUsers.Where(query => query.UserName.Equals(model.UserName) && (compared || query.PasswordHash.Equals(model.Password))).SingleOrDefault();

                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }

        [AllowAnonymous]
        public JsonResult IsEmailAlreadyExist(string email)
        {
            SiteUser user = new SiteUser();

            using (var context = new E_HealthCareEntities())
            {
                user = context.SiteUsers.Where(query => query.Email.ToLower() == email.ToLower()).FirstOrDefault();
            }
            bool status;

            if (user != null)
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
            SiteUser loginDetail = new SiteUser();

            using (var context = new E_HealthCareEntities())
            {
                loginDetail = context.SiteUsers.Where(query => query.UserName.ToLower() == UserName.ToLower()).FirstOrDefault();
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

        [AllowAnonymous]
        public ActionResult DeleteAccount()
        {
            return View();
        }
    }
}