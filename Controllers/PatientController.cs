using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;
using E_HealthCare_Web.ModelSevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult UploadImage(HttpPostedFileBase imageUpload, PatientHomeViewModel model)
        {
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
                return RedirectToAction("PatientHome", new { id = getPatient.p_id });
            }
            return RedirectToAction("PatientHome",new { id = getPatient.p_id});
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

        public ActionResult Appointment()
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