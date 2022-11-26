using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;
using E_HealthCare_Web.ModelSevices;
using System.IO;
using PasswordHashTool;
using PagedList;

namespace E_HealthCare_Web.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        E_HealthCareEntities context = new E_HealthCareEntities();
        DoctorService doctorService = new DoctorService();
        public ActionResult DoctorHome(int id)
        {
            var getDoctor = context.Doctors.Where(q => q.Id == id).FirstOrDefault();
            var ExpiredAppointments = getDoctor.Appointments.Where(q => q.AppointmentDate < DateTime.Now).ToList();
            if (ExpiredAppointments.Count > 0)
            {
                foreach (var appointment in ExpiredAppointments)
                {
                    appointment.IsAppointmentActive = false;
                }
                context.SaveChanges();
            }
            var model = doctorService.DoctorHomeModelTransfer(getDoctor);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(HttpPostedFileBase imageUpload, DoctorHomeViewModel model, string viewActionName)
        {
            string actionName = viewActionName;
            var getDoctor = context.Doctors.Where(q => q.D_UserName == model.UserName).FirstOrDefault();
            if (imageUpload != null)
            {
                var allowedExtension = new[] { ".jpg", ".jpeg", ".png" };
                var imageName = Path.GetFileName(imageUpload.FileName);
                var extension = Path.GetExtension(imageUpload.FileName).ToLower();

                if (allowedExtension.Contains(extension))
                {
                    string fileToDelete = getDoctor.ProfileImagePath;
                    var absolutePath = HttpContext.Server.MapPath(fileToDelete);
                    bool ImageExist = System.IO.File.Exists(absolutePath);
                    if (ImageExist)
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                    string name = Path.GetFileNameWithoutExtension(imageUpload.FileName);
                    string GeneratingImageUrl = "~/Images/ProfilePictures/" + name + "DoctorID" + getDoctor.Id.ToString() + extension;
                    imageUpload.SaveAs(Server.MapPath(GeneratingImageUrl));
                    getDoctor.ProfileImagePath = GeneratingImageUrl;
                    context.SaveChanges();
                }
                return RedirectToAction(actionName, new { id = getDoctor.Id });
            }
            return RedirectToAction(actionName, new { id = getDoctor.Id });
        }


        public ActionResult Edit(int id)
        {
            var getDoctor = context.Doctors.Where(q => q.Id == id).FirstOrDefault();
            var model = doctorService.EditModelTransfer(getDoctor);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(EditDoctorViewModel model)
        {
            var getDoctor = context.Doctors.Where(q => q.Id == model.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                doctorService.UpdateDoctor(getDoctor, model);
                context.SaveChanges();
                return RedirectToAction("DoctorHome", new { id = model.Id });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult CancelAppointment(int appointmentId, string actionName)
        {
            int doctorId = context.Appointments.Where(q => q.id == appointmentId).Select(s => s.DoctorId).FirstOrDefault();
            ViewBag.appointmentId = appointmentId;
            ViewBag.doctorId = doctorId;
            ViewBag.ActionName = actionName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CancelAppointment")]
        public ActionResult CancelAppointmentPost(int id, string actionName)
        {
            var getAppointment = context.Appointments.Where(q => q.id == id).FirstOrDefault();
            var doctorId = getAppointment.DoctorId;
            context.Appointments.Remove(getAppointment);
            context.SaveChanges();
            return RedirectToAction(actionName, new { id = doctorId });
        }

        public ActionResult DoctorProfile(int id)
        {
            var getDoctor = context.Doctors.Where(q => q.Id == id).FirstOrDefault();
            var model = doctorService.DoctorProfileModelTransfer(getDoctor);
            return View(model);
        }

        public ActionResult DoctorAccount(int id)
        {
            var getDoctor = context.Doctors.Where(q => q.Id == id).FirstOrDefault();
            var model = doctorService.AccountModelTransfer(getDoctor);
            return View(model);
        }

        public ActionResult AppointmentList(int id, string sortOrder, string currentfilter, int? page, string SearchByPatientName)
        {
            ViewBag.doctorId = id;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortOrder = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TimeSortOrder = sortOrder == "Time" ? "time_desc" : "Time";

            if (SearchByPatientName != null)
            {
                page = 1;
            }
            else
            {
                SearchByPatientName = currentfilter;
            }

            ViewBag.CurrentFilter = SearchByPatientName;


            var model = context.Appointments.Where(q => q.DoctorId == id).ToList().Select(q => new DoctorAppointmentListViewModel
            {
                Id = q.id,
                appointmentDate = q.AppointmentDate.Date,
                appointmentTime = DateTime.Parse(q.AppointmentDate.ToString()),
                departmentName = q.Department.DepartmentName,
                patientName = (q.Patient.p_name != null) ? q.Patient.p_name : q.Patient.UserName

            });

            if (!String.IsNullOrEmpty(SearchByPatientName))
            {
                model = model.Where(s => s.patientName.ToUpper().Contains(SearchByPatientName.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Date":
                    model = model.OrderBy(o => o.appointmentDate);
                    break;
                case "date_desc":
                    model = model.OrderByDescending(o => o.appointmentDate);
                    break;
                case "time_desc":
                    model = model.OrderByDescending(o => o.appointmentTime).ThenByDescending(w => w.appointmentDate);
                    break;
                case "Time":
                    model = model.OrderBy(o => o.appointmentTime).ThenBy(w => w.appointmentDate);
                    break;
                default:
                    model = model.OrderBy(o => o.Id);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult ChangePassword(int id)
        {
            var doctorId = id;
            var getUser = context.Doctors.Where(q => q.Id == doctorId).Select(q => q.D_UserName).FirstOrDefault();
            var getPasswordInfo = context.SiteUsers.Where(q => q.UserName == getUser).Select(q => q.PasswordHash).FirstOrDefault();
            var passwordModel = doctorService.PasswordModelTransfer(doctorId, getPasswordInfo);
            return View(passwordModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(DoctorPasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getUserName = context.Doctors.Where(q => q.Id == model.id).Select(q => q.D_UserName).FirstOrDefault();
                var getUser = context.SiteUsers.Where(q => q.UserName == getUserName).FirstOrDefault();

                if (getUser != null)
                {
                    getUser.PasswordHash = PasswordHashManager.CreateHash(model.ConfirmNewPassword);
                    context.Configuration.ValidateOnSaveEnabled = false; //to avoid validation issue
                    context.SaveChanges();
                    return RedirectToAction("DoctorHome", new { id = model.id });
                }

            }
            return View(model);
        }

        //public ActionResult PatientList()
        //{
        //    return View();
        //}

        public ActionResult Chat(int id, string searchp)
        {
            ViewBag.doctorId = id.ToString();
            DoctorConsultationViewModel model = new DoctorConsultationViewModel();
            model.id = id;
            if (!String.IsNullOrEmpty(searchp))
            {
                
                model.PatientsList = context.Patients.Where(q => q.p_name != null ? q.p_name.ToUpper().Contains(searchp.ToUpper()): q.UserName.ToUpper().Contains(searchp.ToUpper())).ToList();
            }
            else
            {
                model.PatientsList = context.Patients.ToList();
            }
            model.CurrentDoctor = context.Doctors.Where(q => q.Id == id).FirstOrDefault();

            return View(model);
        }


        public ActionResult ChatBox(string patientUserName, string doctorUserName)
        {

            DoctorChatLoadViewModel model = new DoctorChatLoadViewModel();            
            model.patient = context.Patients.Where(q => q.UserName == patientUserName).FirstOrDefault();
            model.doctor = context.Doctors.Where(q => q.D_UserName == doctorUserName).FirstOrDefault();
            model.Message = context.ConversationInfoes.Where(q => q.SenderId == model.doctor.D_UserId && q.RecieverId == model.patient.UserId || q.SenderId == model.patient.UserId && q.RecieverId == model.doctor.D_UserId)?.ToList().Select(q =>
            new Message
            {
                IsDoctorMsg = (q.SenderId == model.doctor.D_UserId && q.RecieverId == model.patient.UserId),
                IsPatientMsg = (q.SenderId == model.patient.UserId && q.RecieverId == model.doctor.D_UserId),
                MessageText = q.MessageText
            });
            return PartialView("_ChatBox", model);
        }


        //Helper Methods
        public JsonResult IsCorrectPassword(string OldPassword, int id)
        {
            var getUserName = context.Doctors.Where(q => q.Id == id).Select(q => q.D_UserName).FirstOrDefault();
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