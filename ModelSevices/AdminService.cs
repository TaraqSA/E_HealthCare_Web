using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_HealthCare_Web.ViewModels;
using E_HealthCare_Web.Models;

namespace E_HealthCare_Web.ModelSevices
{
    public class AdminService
    {
        E_HealthCareEntities context = new E_HealthCareEntities();
        public AdminHomeViewModel AdminHomeModelTransfer(Admin admin)
        {
            AdminHomeViewModel model = new AdminHomeViewModel();

            model.Address = admin.Address;
            model.AdminName = admin.Name;
            model.BloodGroup = admin.BloodGroup;
            model.DateOfBirth = admin.DateOfBirth;
            model.EmailAddress = admin.Email;
            model.Gender = admin.Gender;
            model.Id = admin.Id;            
            model.Phone = admin.PhoneNumber;
            model.ProfileImagePath = admin.ProfileImagePath;
            model.UserName = admin.UserName;            
            model.DepartmentList = context.Departments.Count();
            model.DoctorList = context.Doctors.Count();
            model.PatientList = context.Patients.Count();

            return model;
        }


        public AdminEditViewModel AdminEditModelTransfer(Admin admin)
        {
            AdminEditViewModel model = new AdminEditViewModel();
            model.Address = admin.Address;
            model.AdminName = admin.Name;
            model.BloodGroup = admin.BloodGroup;
            model.DateOfBirth = admin.DateOfBirth;
            model.Gender = admin.Gender;
            model.Id = admin.Id;
            model.Phone = admin.PhoneNumber;            
            return model;
        }

        public void UpdateAdmin(AdminEditViewModel model, Admin admin)
        {
            admin.Name = model.AdminName;
            admin.PhoneNumber = model.Phone;
            admin.Address = model.Address;
            admin.BloodGroup = model.BloodGroup;
            admin.DateOfBirth = model.DateOfBirth;
            admin.Gender = model.Gender;
        }

        public AdminProfileViewModel AdminProfileModelTransfer(Admin admin)
        {
            AdminProfileViewModel model = new AdminProfileViewModel();
            model.Address = admin.Address;
            model.Name = admin.Name;
            model.BloodGroup = admin.BloodGroup;
            model.DOB = admin.DateOfBirth;
            model.Email = admin.Email;
            model.Gender = admin.Gender;
            model.Id = admin.Id;
            model.Phone = admin.PhoneNumber;
            model.ProfileImagePath = admin.ProfileImagePath;
            model.UserName = admin.UserName;
            return model;
        }

        public AdminAccountViewModel AccountModelTransfer(Admin admin)
        {
            AdminAccountViewModel model = new AdminAccountViewModel();
            model.id = admin.Id;
            model.UserName = admin.UserName;
            model.IsEmailVerified = admin.IsEmailVerified;
            model.Email = admin.Email;
            return model;
        }

        public AdminPasswordChangeViewModel PasswordModelTransfer(int adminId, string Password)
        {
            AdminPasswordChangeViewModel model = new AdminPasswordChangeViewModel();
            model.id = adminId;
            model.OldPassword = Password;
            return model;
        }

    }

}