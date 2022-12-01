using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;

namespace E_HealthCare_Web.ModelSevices
{
    public class DoctorService
    {
        public DoctorHomeViewModel DoctorHomeModelTransfer(Doctor doctor)
        {
            DoctorHomeViewModel model = new DoctorHomeViewModel();
            model.Id = doctor.Id;
            model.EmailAddress = doctor.D_Email;
            model.DateOfBirth = doctor.D_DateOfBirth;
            model.DoctorName = doctor.D_Name;
            model.ProfileImagePath = doctor.ProfileImagePath;
            model.UserName = doctor.D_UserName;
            model.Speciality = doctor.Departments.ToList();
            model.Appointments = doctor.Appointments.Where(q=>q.IsAppointmentActive && (q.AppointmentDate - DateTime.Now).Days < 3 ).ToList();
            model.Gender = doctor.D_Gender;
            model.Phone = doctor.D_Phone;
            model.BloodGroup = doctor.D_BloodGroup;
            model.Address = doctor.D_Address;
            return model;
        }

        public EditDoctorViewModel EditModelTransfer(Doctor doctor)
        {
            EditDoctorViewModel editViewModel = new EditDoctorViewModel();
            editViewModel.Id = doctor.Id;
            editViewModel.DoctorName = doctor.D_Name;
            editViewModel.Gender = doctor.D_Gender;
            editViewModel.BloodGroup = doctor.D_BloodGroup;
            editViewModel.DateOfBirth = doctor.D_DateOfBirth;
            editViewModel.Phone = doctor.D_Phone;
            editViewModel.Address = doctor.D_Address;
            return editViewModel;
        }

        public void UpdateDoctor(Doctor doctor, EditDoctorViewModel model)
        {
            doctor.D_Name = model.DoctorName;
            doctor.D_DateOfBirth = model.DateOfBirth;
            doctor.D_Address = model.Address;
            doctor.D_BloodGroup = model.BloodGroup;
            doctor.D_Phone = model.Phone;
            doctor.D_Gender = model.Gender;
        }

        public DoctorProfileViewModel DoctorProfileModelTransfer(Doctor doctor)
        {
            DoctorProfileViewModel model = new DoctorProfileViewModel();
            model.Id = doctor.Id;
            model.Name = doctor.D_Name;
            model.Phone = doctor.D_Phone;
            model.ProfileImagePath = doctor.ProfileImagePath;
            model.UserName = doctor.D_UserName;
            model.Email = doctor.D_Email;
            model.DOB = doctor.D_DateOfBirth;
            model.BloodGroup = doctor.D_BloodGroup;
            model.Address = doctor.D_Address;
            model.Gender = doctor.D_Gender;
            return model;
        }
        public DoctorAccountViewModel AccountModelTransfer(Doctor doctor)
        {
            DoctorAccountViewModel doctorAccountViewModel = new DoctorAccountViewModel();
            doctorAccountViewModel.id = doctor.Id;
            doctorAccountViewModel.UserName = doctor.D_UserName;
            doctorAccountViewModel.IsEmailVerified = doctor.IsEmailVerified;
            doctorAccountViewModel.Email = doctor.D_Email;
            return doctorAccountViewModel;
        }
        public DoctorPasswordChangeViewModel PasswordModelTransfer(int doctorId, string Password)
        {
            DoctorPasswordChangeViewModel model = new DoctorPasswordChangeViewModel();
            model.id = doctorId;
            model.OldPassword = Password;
            return model;
        }
    }
}