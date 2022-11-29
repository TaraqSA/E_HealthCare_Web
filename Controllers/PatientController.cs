using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;
using E_HealthCare_Web.ModelSevices;
using System;
using PagedList;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PasswordHashTool;

namespace E_HealthCare_Web.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        // GET: Patient

        E_HealthCareEntities context = new E_HealthCareEntities();
        PatientService patientService = new PatientService();
        public ActionResult PatientHome(int id)
        {
            var getPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();
            var model = patientService.PatientHomeModelTransfer(getPatient);
            var ExpiredAppointments = getPatient.Appointments.Where(q => q.AppointmentDate < DateTime.Now).ToList();
            if (ExpiredAppointments.Count > 0)
            {
                foreach (var appointment in ExpiredAppointments)
                {
                    appointment.IsAppointmentActive = false;
                }
                context.SaveChanges();
            }
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
            var model = patientService.EditModelTransfer(getPatient);
            return View("EditPatient", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            var getPatient = context.Patients.Where(q => q.p_id == model.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                patientService.UpdatePatient(getPatient, model);
                context.SaveChanges();
                return RedirectToAction("PatientHome", "Patient", new { id = model.Id });
            }

            return View("EditPatient", model);
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


        public ActionResult BookAppointment(int id)
        {
            var model = patientService.BookAppointmentModelTransfer(id);
            return View(model);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FillDoctors(int? DeptId)
        {
            var doctors = context.Departments.Where(q => q.Id == DeptId).Select(q => q.Doctors).FirstOrDefault();
            var doctorData = doctors.Select(m => new SelectListItem() { Text = m.D_UserName, Value = m.Id.ToString() });
            return Json(doctorData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment(BookAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                patientService.AddAppointmentToDataBase(model);
                return RedirectToAction("AppointmentList", new { id = model.Id });
            }
            return View();
        }

        public ActionResult AppointmentList(int id, string sortOrder, string currentfilter, int? page, string SearchByDrName)
        {
            ViewBag.PatientId = id;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortOrder = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TimeSortOrder = sortOrder == "Time" ? "time_desc" : "Time";

            if (SearchByDrName != null)
            {
                page = 1;
            }
            else
            {
                SearchByDrName = currentfilter;
            }

            ViewBag.CurrentFilter = SearchByDrName;


            var model = context.Appointments.Where(q => q.PatientId == id && q.IsAppointmentActive == true).ToList().Select(q => new AppointmentListViewModel
            {
                id = q.id,
                appointmentDate = q.AppointmentDate.Date,
                appointmentTime = DateTime.Parse(q.AppointmentDate.ToString()),
                departmentName = q.Department.DepartmentName,
                doctorName = q.Doctor.D_UserName

            });

            if (!String.IsNullOrEmpty(SearchByDrName))
            {
                model = model.Where(s => s.doctorName.ToUpper().Contains(SearchByDrName.ToUpper()));
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
                    model = model.OrderBy(o => o.id);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult CancelAppointment(int id)
        {
            int patientId = context.Appointments.Where(q => q.id == id).Select(s => s.PatientId).FirstOrDefault();
            ViewBag.appointmentId = id;
            ViewBag.patientId = patientId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CancelAppointment")]
        public ActionResult CancelAppointmentAfterConfirm(int id)
        {
            var getAppointment = context.Appointments.Where(q => q.id == id).FirstOrDefault();
            int patientId = getAppointment.PatientId;
            context.Appointments.Remove(getAppointment);
            context.SaveChanges();
            return RedirectToAction("AppointmentList", new { id = patientId });
        }


        public ActionResult PatientProfile(int id)
        {
            var getPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();
            PatientService patientService = new PatientService();
            var model = patientService.PatientProfileModelTransfer(getPatient);
            return View(model);
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
            var passwordModel = patientService.PasswordModelTransfer(id, getPasswordInfo);
            return View(passwordModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(PatientPasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getUserName = context.Patients.Where(q => q.p_id == model.id).Select(q => q.UserName).FirstOrDefault();
                var getUser = context.SiteUsers.Where(q => q.UserName == getUserName).FirstOrDefault();

                if (getUser != null)
                {
                    getUser.PasswordHash = PasswordHashManager.CreateHash(model.ConfirmNewPassword);
                    context.Configuration.ValidateOnSaveEnabled = false; //to avoid validation issue
                    context.SaveChanges();
                    return RedirectToAction("PatientHome", new { id = model.id });
                }

            }
            return View(model);
        }

        [HttpGet]
        public ActionResult UploadReports(int id)
        {
            var model = new UploadReportViewModel();
            model.id = id;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadReports(UploadReportViewModel model, HttpPostedFileBase pdfUpload)
        {
            if (ModelState.IsValid)
            {
                var getPatient = context.Patients.Where(q => q.p_id == model.id).FirstOrDefault();
                PatientReport PatientHealthReport = new PatientReport();

                if (pdfUpload != null)//saving pdf file report address to database
                {
                    var allowedExtension = ".pdf";
                    var fileName = Path.GetFileName(pdfUpload.FileName);
                    var extension = Path.GetExtension(pdfUpload.FileName).ToLower();

                    if (extension != allowedExtension)
                    {
                        return View();
                    }
                    else
                    {
                        string name = Path.GetFileNameWithoutExtension(pdfUpload.FileName);
                        string GeneratingFileUrl = @"~\HealthReports\" + name + "PatientID" + getPatient.p_id.ToString() + extension;
                        pdfUpload.SaveAs(Server.MapPath(GeneratingFileUrl));
                        string addressToSave = GeneratingFileUrl.Substring(1);
                        PatientHealthReport.ReportAddress = GeneratingFileUrl.Substring(1);
                        PatientHealthReport.ReportType = model.ReportTypes.ToString();
                        PatientHealthReport.ReportName = model.ReportName;
                        PatientHealthReport.Patient = getPatient;
                        context.PatientReports.Add(PatientHealthReport);
                        context.SaveChanges();
                        return RedirectToAction("ReportList", new { id = model.id });
                    }
                }

            }
            return View();
        }


        public ActionResult ReportList(int id, string searchByName, string currentfilter, int? page)
        {
            //searching and paging
            if (searchByName != null)
            {
                page = 1;
            }
            else
            {
                searchByName = currentfilter;
            }

            ViewBag.CurrentFilter = searchByName;

            var model = context.PatientReports.Where(q => q.PatientId == id).ToList().Select(q => new ReportListViewModel
            {
                id = q.ReportId,
                reportType = q.ReportType,
                fileLink = q.ReportAddress,
                reportName = q.ReportName

            });

            if (!String.IsNullOrEmpty(searchByName))
            {
                model = model.Where(q => q.reportName.ToUpper().Contains(searchByName.ToUpper()));
            }

            ViewBag.PatientId = id;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ViewReport(int id)
        {
            var getReport = context.PatientReports.Where(q => q.ReportId == id).FirstOrDefault();
            string fileAddress = Path.GetFullPath(@"C:\Users\HOME\source\repos\E_HealthCare_Web\" + getReport.ReportAddress);
            return File(fileAddress, "application/pdf");
        }

        public ActionResult DownloadReport(int id)
        {
            var getReportAddress = context.PatientReports.Where(q => q.ReportId == id).Select(q => q.ReportAddress).FirstOrDefault();
            if (getReportAddress != null)
            {
                string fullFileAddress = Path.GetFullPath(@"C:\Users\HOME\source\repos\E_HealthCare_Web\" + getReportAddress);
                return File(fullFileAddress, "application/pdf", "MyReport.pdf");
            }

            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReport(int reportId)
        {
            var getReport = context.PatientReports.Where(q => q.ReportId == reportId).FirstOrDefault();
            if (getReport != null)
            {
                int patienId = getReport.PatientId;
                context.PatientReports.Remove(getReport);
                context.SaveChanges();
                return RedirectToAction("ReportList", new { id = patienId });
            }
            return null;
        }



        public ActionResult FindDoctors(int id, string DrSearch, FindDoctorViewModel Recievedmodel)
        {
            var model = patientService.FindDoctorModelTansfer(Recievedmodel, Recievedmodel.DepartmentSelectedId, id, DrSearch);
            return View(model);
        }

        public ActionResult Chat(int id, string searchdr)
        {
            ViewBag.patiendId = id.ToString();
            ConsultationViewModel model = new ConsultationViewModel();
            model.id = id;
            if (!String.IsNullOrEmpty(searchdr))
            {
                model.DoctorsList = context.Doctors.Where(q => q.D_Name != null ? q.D_Name.ToUpper().Contains(searchdr.ToUpper()) : q.D_UserName.ToUpper().Contains(searchdr.ToUpper())).ToList();
            }
            else
            {
                model.DoctorsList = context.Doctors.ToList();
            }
            model.CurrentPatient = context.Patients.Where(q => q.p_id == id).FirstOrDefault();

            return View(model);
        }


        public ActionResult ChatBox(string patientUserName, string doctorUserName)
        {
            ChatLoadViewModel model = new ChatLoadViewModel();
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

        public JsonResult IsDateValid(DateTime appointmentDate)
        {
            bool status;
            var CurrentDate = DateTime.Now;

            if (appointmentDate < CurrentDate || appointmentDate.DayOfWeek == 0 || appointmentDate.DayOfWeek > DayOfWeek.Friday)
            {
                status = false;
            }
            else
            {
                status = true;
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTimeValid(DateTime appointmentTime)
        {
            bool status;
            if (appointmentTime.TimeOfDay.Hours >= 9 && appointmentTime.TimeOfDay.Hours <= 17)
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