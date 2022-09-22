using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;
using E_HealthCare_Web.ModelSevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PasswordHashTool;

namespace E_HealthCare_Web.Controllers
{
    [Authorize]

    public class PatientController : Controller
    {
        // GET: Patient

        E_HealthCareEntities context = new E_HealthCareEntities();
        public ActionResult PatientHome(int id)
        {
            var getPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();
            PatientService patientService = new PatientService();
            var model = patientService.PatientHomeModelTransfer(getPatient);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(HttpPostedFileBase imageUpload, PatientHomeViewModel model, string viewActionName)
        {   
            string actionName = viewActionName;
            var getPatient = context.Patients.Where(q => q.UserName == model.UserName).FirstOrDefault();
            if (imageUpload != null)
            {
                var allowedExtension = new[] { ".jpg", ".jpeg", ".png" };
                var imageName = Path.GetFileName(imageUpload.FileName);
                var extension = Path.GetExtension(imageUpload.FileName).ToLower();

                if (allowedExtension.Contains(extension))
                {
                    string fileToDelete = getPatient.ProfileImagePath;
                    var absolutePath = HttpContext.Server.MapPath(fileToDelete);
                    bool ImageExist = System.IO.File.Exists(absolutePath);
                    if (ImageExist)
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                    string name = Path.GetFileNameWithoutExtension(imageUpload.FileName);
                    string GeneratingImageUrl = "~/Images/ProfilePictures/" + name + "PatientID" + getPatient.p_id.ToString() + extension;
                    imageUpload.SaveAs(Server.MapPath(GeneratingImageUrl));
                    getPatient.ProfileImagePath = GeneratingImageUrl;
                    context.SaveChanges();
                }
                return RedirectToAction(actionName, new { id = getPatient.p_id });
            }
            return RedirectToAction(actionName, new { id = getPatient.p_id });
        }

        public ActionResult Edit(int id)
        {
            var getPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();
            PatientService patientService = new PatientService();
            var model = patientService.EditModelTransfer(getPatient);
            return View("EditPatient", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            var getPatient = context.Patients.Where(q => q.p_id == model.Id).FirstOrDefault();
            PatientService patientService = new PatientService();
            if (ModelState.IsValid)
            {
                patientService.UpdatePatient(getPatient, model);
                context.SaveChanges();
            }

            return RedirectToAction("PatientHome", "Patient", new { id = model.Id });
        }


        [HttpGet]
        public ActionResult BMICalculator()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BMICalculator(FormCollection form)
        {
            if (!(String.IsNullOrEmpty(form["Weight"]) || String.IsNullOrEmpty(form["feet"]) || String.IsNullOrEmpty(form["Inch"])))
            {
                //BMI = (Weight in Pounds / (Height in inches x Height in inches)) x 703
                //BMI = (Weight in Kilograms / (Height in Meters x Height in Meters))
                //1 foot = 0.3048 meters  or 12inch
                //1 inch = 0.0254 meters

                double heightConvertFromFeetToInchToMeters = ((double.Parse(form["feet"]) * 12) + double.Parse(form["Inch"])) * 0.0254;
                double weight = int.Parse(form["Weight"]);

                var BMI = weight / (heightConvertFromFeetToInchToMeters * heightConvertFromFeetToInchToMeters);

                ViewBag.BMIResult = Math.Round(BMI, 2);

                return View();
            }

            return View();
        }


        public ActionResult Appointment()
        {
            return View();
        }

        public ActionResult PatientProfile(int id)
        {
            var getPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();
            PatientService patientService = new PatientService();
            var model = patientService.PatientProfileModelTransfer(getPatient);
            return View(model);
        }
        public ActionResult AvailableDoctors()
        {
            return View();
        }



        public ActionResult PatientAccount(int id)
        {
            var getPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();

            PatientService patientService = new PatientService();
            var model = patientService.AccountModelTransfer(getPatient);
            return View(model);
        }

        public ActionResult ChangePassword(int id)
            {
            var getUser = context.Patients.Where(q => q.p_id == id).Select(q => q.UserName).FirstOrDefault();
            var getPasswordInfo = context.SiteUsers.Where(q => q.UserName == getUser).Select(q => q.PasswordHash).FirstOrDefault();
            PatientService patientService = new PatientService();
            var passwordModel= patientService.PasswordModelTransfer(id, getPasswordInfo);
            return View(passwordModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(PatientPasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getUserName = context.Patients.Where(q => q.p_id == model.id).Select(q => q.UserName).FirstOrDefault();
                var getUser = context.SiteUsers.Where(q => q.UserName == getUserName).FirstOrDefault();

                if(getUser != null)
                {
                    getUser.PasswordHash = PasswordHashManager.CreateHash(model.ConfirmNewPassword);
                    context.Configuration.ValidateOnSaveEnabled = false; //to avoid validation issue
                    context.SaveChanges();
                    return RedirectToAction("PatientHome", new { id = model.id });
                }
                
            }
            return View(model);
        }


        public JsonResult IsCorrectPassword(string OldPassword, int id)
        {   
            var getUserName = context.Patients.Where(q => q.p_id == id).Select(q => q.UserName).FirstOrDefault();
            var getPassword = context.SiteUsers.Where(q => q.UserName == getUserName).Select(q => q.PasswordHash).FirstOrDefault();
            bool status;
            if (PasswordHashManager.ValidatePassword(OldPassword, getPassword))
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}