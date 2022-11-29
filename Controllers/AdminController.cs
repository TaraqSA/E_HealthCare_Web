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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        E_HealthCareEntities dbContext = new E_HealthCareEntities();
        AdminService AdminServices = new AdminService();
        public ActionResult Index(int id)
        {
            var getAdmin = dbContext.Admins.Where(q => q.Id == id).FirstOrDefault();
            var model = AdminServices.AdminHomeModelTransfer(getAdmin);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(HttpPostedFileBase imageUpload, AdminHomeViewModel model, string viewActionName)
        {
            string actionName = viewActionName;
            var getAdmin = dbContext.Admins.Where(q => q.UserName == model.UserName).FirstOrDefault();
            if (imageUpload != null)
            {
                var allowedExtension = new[] { ".jpg", ".jpeg", ".png" };
                var imageName = Path.GetFileName(imageUpload.FileName);
                var extension = Path.GetExtension(imageUpload.FileName).ToLower();

                if (allowedExtension.Contains(extension))
                {
                    string fileToDelete = getAdmin.ProfileImagePath;
                    var absolutePath = HttpContext.Server.MapPath(fileToDelete);
                    bool ImageExist = System.IO.File.Exists(absolutePath);
                    if (ImageExist)
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                    string name = Path.GetFileNameWithoutExtension(imageUpload.FileName);
                    string GeneratingImageUrl = "~/Images/ProfilePictures/" + name + "AdminID" + getAdmin.Id.ToString() + extension;
                    imageUpload.SaveAs(Server.MapPath(GeneratingImageUrl));
                    getAdmin.ProfileImagePath = GeneratingImageUrl;
                    dbContext.SaveChanges();
                }
                return RedirectToAction(actionName, new { id = getAdmin.Id });
            }
            return RedirectToAction(actionName, new { id = getAdmin.Id });
        }

        public ActionResult Edit(int id)
        {
            var getAdmin = dbContext.Admins.Where(q => q.Id == id).FirstOrDefault();
            var model = AdminServices.AdminEditModelTransfer(getAdmin);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdminEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getAdmin = dbContext.Admins.Where(q => q.Id == model.Id).FirstOrDefault();
                AdminServices.UpdateAdmin(model, getAdmin);
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { id = model.Id });
            }
            return View(model);
        }


        public ActionResult AdminProfile(int id)
        {
            var getAdmin = dbContext.Admins.Where(q => q.Id == id).FirstOrDefault();
            var model = AdminServices.AdminProfileModelTransfer(getAdmin);
            return View(model);
        }

        public ActionResult AdminAccount(int id)
        {
            var getAdmin = dbContext.Admins.Where(q => q.Id == id).FirstOrDefault();
            var model = AdminServices.AccountModelTransfer(getAdmin);
            return View(model);
        }

        public ActionResult ChangePassword(int id)
        {
            var adminId = id;
            var getUser = dbContext.Admins.Where(q => q.Id == adminId).Select(q => q.UserName).FirstOrDefault();
            var getPasswordInfo = dbContext.SiteUsers.Where(q => q.UserName == getUser).Select(q => q.PasswordHash).FirstOrDefault();
            var passwordModel = AdminServices.PasswordModelTransfer(adminId, getPasswordInfo);
            return View(passwordModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(DoctorPasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getUserName = dbContext.Admins.Where(q => q.Id == model.id).Select(q => q.UserName).FirstOrDefault();
                var getUser = dbContext.SiteUsers.Where(q => q.UserName == getUserName).FirstOrDefault();

                if (getUser != null)
                {
                    getUser.PasswordHash = PasswordHashManager.CreateHash(model.ConfirmNewPassword);
                    dbContext.Configuration.ValidateOnSaveEnabled = false; //to avoid validation issue
                    dbContext.SaveChanges();
                    return RedirectToAction("Index", new { id = model.id });
                }

            }
            return View(model);
        }

        public ActionResult AdminList(int? id, string currentfilter, int? page, string SearchByAdminName)
        {
            if (id != null)
            {
                ViewBag.adminId = id;
            }

            if (SearchByAdminName != null)
            {
                page = 1;
            }
            else
            {
                SearchByAdminName = currentfilter;
            }

            ViewBag.CurrentFilter = SearchByAdminName;


            var model = dbContext.Admins.Where(q => q.Id != id).ToList().Select(q => new AdminListViewModel
            {
                Id = q.Id,
                Name = q.Name == null ? "Unavailable" : q.Name,
                Gender = q.Gender,
                Phone = q.PhoneNumber,
                Email = q.Email

            });

            if (!String.IsNullOrEmpty(SearchByAdminName))
            {
                model = model.Where(s => s.Name.ToUpper().Contains(SearchByAdminName.ToUpper()));
            }


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateAdmin(int id)
        {
            CreateAdminViewModel model = new CreateAdminViewModel();
            model.Id = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAdmin(CreateAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var HashedPassword = PasswordHashManager.CreateHash(model.Password);
                var getRole = dbContext.Roles.Where(q => q.RoleName == "Admin").ToList();

                Admin admin = new Admin();
                SiteUser siteUser = new SiteUser();

                //adding columns
                admin.Name = model.AdminName;
                admin.UserName = model.UserName;
                admin.Email = model.Email;
                siteUser.Email = model.Email;
                siteUser.UserName = model.UserName;
                siteUser.UserRole = "Admin";
                siteUser.PasswordHash = HashedPassword;

                //adding relational properties
                siteUser.Roles = getRole;
                admin.SiteUser = siteUser;

                //saving to database 
                dbContext.SiteUsers.Add(siteUser);
                dbContext.Admins.Add(admin);
                dbContext.SaveChanges();
                return RedirectToAction("AdminList", new { id = model.Id });
            }

            return View(model);
        }

        public ActionResult RemoveAdmin(int adminId)
        {
            var getAdminToRemove = dbContext.Admins.Find(adminId);
            var currentAdminId = dbContext.Admins.Where(q => q.UserName == User.Identity.Name).Select(q => q.Id).FirstOrDefault();
            ViewBag.CurrentAdminId = currentAdminId;
            ViewBag.AdminId = adminId;
            ViewBag.AdminName = getAdminToRemove.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAdmin(int id, int currentAdminId)
        {
            var getAdmin = dbContext.Admins.Find(id);
            var getRole = getAdmin.SiteUser.Roles.Where(q => q.RoleName == "Admin").FirstOrDefault();

            getAdmin.SiteUser.Roles.Remove(getRole);
            dbContext.SiteUsers.Remove(getAdmin.SiteUser);
            dbContext.Admins.Remove(getAdmin);
            dbContext.SaveChanges();

            return RedirectToAction("AdminList", new { id = currentAdminId });
        }

        public ActionResult DoctorsList(int id, string currentfilter, int? page, string SearchByDoctorName)
        {
            ViewBag.adminId = id;

            if (SearchByDoctorName != null)
            {
                page = 1;
            }
            else
            {
                SearchByDoctorName = currentfilter;
            }

            ViewBag.CurrentFilter = SearchByDoctorName;


            var model = dbContext.Doctors.ToList().Select(q => new DoctorsListViewModel
            {
                Id = q.Id,
                Name = q.D_Name == null? "Unavailable" :q.D_Name,
                Gender = q.D_Gender,
                Phone = q.D_Phone,
                Email = q.D_Email

            });

            if (!String.IsNullOrEmpty(SearchByDoctorName))
            {
                model = model.Where(s => s.Name.ToUpper().Contains(SearchByDoctorName.ToUpper()));
            }


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RemoveDoctor(int doctorId)
        {
            var getdoctorToRemove = dbContext.Doctors.Find(doctorId);
            var currentAdminId = dbContext.Admins.Where(q => q.UserName == User.Identity.Name).Select(q => q.Id).FirstOrDefault();
            ViewBag.CurrentAdminId = currentAdminId;
            ViewBag.DoctorId = doctorId;
            ViewBag.DoctorName = getdoctorToRemove.D_Name;            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RemoveDoctor")]
        public ActionResult RemoveDoctorPost(int doctorId)
        {
            var getAdminId = dbContext.Admins.Where(q => q.UserName == User.Identity.Name).Select(q => q.Id).FirstOrDefault();
            var getDoctor = dbContext.Doctors.Find(doctorId);
            var getAppointments = getDoctor.Appointments.ToList();
            var getDepartments = getDoctor.Departments;
            var getSiteUser = getDoctor.SiteUser;
            var getRoles = getSiteUser.Roles;
            foreach(var appoinment in getAppointments)
            {
                dbContext.Appointments.Remove(appoinment);
            }
            foreach(var dept in getDepartments.ToList())
            {
                getDepartments.Remove(dept);
            }
            foreach(var role in getRoles.ToList())
            {
                getRoles.Remove(role);
            }

            dbContext.SiteUsers.Remove(getSiteUser);
            dbContext.Doctors.Remove(getDoctor);
            dbContext.SaveChanges();
            return RedirectToAction("DoctorsList", new { id = getAdminId });
        }

        public ActionResult DepartmentList(int id, string currentfilter, int? page, string SearchByDepartmentName)
        {
            ViewBag.adminId = id;

            if (SearchByDepartmentName != null)
            {
                page = 1;
            }
            else
            {
                SearchByDepartmentName = currentfilter;
            }

            ViewBag.CurrentFilter = SearchByDepartmentName;


            var model = dbContext.Departments.ToList().Select(s => new DepartmentListViewModel {Id = s.Id, DepartmentName = s.DepartmentName });

            if (!String.IsNullOrEmpty(SearchByDepartmentName))
            {
                model = model.Where(s => s.DepartmentName.ToUpper().Contains(SearchByDepartmentName.ToUpper()));
            }


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult AddDepartment(int id)
        {
            return View();
        }

        public ActionResult RemoveDepartment(int departmentId)
        {
            return View();
        }

        public ActionResult AddDoctorToDepartment(Doctor doctor)
        {
            return View();
        }


        public ActionResult RemoveDoctorFromDepartment(int doctorId,int departmentId)
        {
            return View();
        }

        //Helper Methods
        public JsonResult IsCorrectPassword(string OldPassword, int id)
        {
            var getUserName = dbContext.Admins.Where(q => q.Id == id).Select(q => q.UserName).FirstOrDefault();
            var getPassword = dbContext.SiteUsers.Where(q => q.UserName == getUserName).Select(q => q.PasswordHash).FirstOrDefault();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}