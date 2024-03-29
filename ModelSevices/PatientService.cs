﻿using E_HealthCare_Web.Models;
using E_HealthCare_Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_HealthCare_Web.ModelSevices
{
    public class PatientService 
    {
        E_HealthCareEntities context = new E_HealthCareEntities();
        public EditViewModel EditModelTransfer(Patient getPatient)
        {
            EditViewModel editViewModel = new EditViewModel();
            editViewModel.Id = getPatient.p_id;
            editViewModel.FullName = getPatient.p_name;
            editViewModel.Gender = getPatient.p_gender;
            editViewModel.Email = getPatient.p_Email;
            editViewModel.BloodGroup = getPatient.p_BloodGroup;
            editViewModel.DOB = getPatient.p_dateOfBirth;
            editViewModel.Phone = getPatient.p_phone;
            editViewModel.Address = getPatient.p_address;
            return editViewModel;
        }


        public PatientHomeViewModel PatientHomeModelTransfer(Patient getPatient)
        {
            PatientHomeViewModel patientHomeViewModel = new PatientHomeViewModel();
            patientHomeViewModel.Id = getPatient.p_id;
            patientHomeViewModel.Name = getPatient.p_name;
            patientHomeViewModel.Phone = getPatient.p_phone;
            patientHomeViewModel.UserName = getPatient.UserName;
            patientHomeViewModel.BloodGroup = getPatient.p_BloodGroup;
            patientHomeViewModel.DOB = getPatient.p_dateOfBirth;
            patientHomeViewModel.Email = getPatient.p_Email;
            patientHomeViewModel.Address = getPatient.p_address;
            patientHomeViewModel.ProfileImagePath = getPatient.ProfileImagePath;
            if (getPatient.p_gender != null && getPatient.p_gender.Length == 1)
            {
                patientHomeViewModel.Gender = ChangeGenderValue(getPatient.p_gender);
            }
            else
            {
                patientHomeViewModel.Gender = getPatient.p_gender;
            }
            return patientHomeViewModel;
        }

        public PatientProfileViewModel PatientProfileModelTransfer(Patient patient)
        {
            PatientProfileViewModel model = new PatientProfileViewModel();
            model.Id = patient.p_id;
            model.Name = patient.p_name;
            model.Phone = patient.p_phone;
            model.ProfileImagePath = patient.ProfileImagePath;
            model.UserName = patient.UserName;
            model.Email = patient.p_Email;
            model.DOB = patient.p_dateOfBirth;
            model.BloodGroup = patient.p_BloodGroup;
            model.Address = patient.p_address;
            if (patient.p_gender != null && patient.p_gender.Length == 1)
            {
                model.Gender = ChangeGenderValue(patient.p_gender);
            }
            else
            {
                model.Gender = patient.p_gender;
            }

            return model;
        }


        public void UpdatePatient(Patient getPatient, EditViewModel model)
        {
            getPatient.p_name = model.FullName;
            getPatient.p_dateOfBirth = model.DOB;
            getPatient.p_address = model.Address;
            getPatient.p_BloodGroup = model.BloodGroup;
            getPatient.p_Email = model.Email;
            getPatient.p_phone = model.Phone;
            getPatient.p_gender = model.Gender;
        }

        public string ChangeGenderValue(string value)
        {
            switch (value)
            {
                case "M":
                    return "Male";
                case "F":
                    return "Female";
                case "O":
                    return "Other";
            }
            return "Unkown";
        }

        public PatientAccountViewModel AccountModelTransfer(Patient patient)
        {
            PatientAccountViewModel patientAccountViewModel = new PatientAccountViewModel();
            patientAccountViewModel.id = patient.p_id;
            patientAccountViewModel.UserName = patient.UserName;
            patientAccountViewModel.IsEmailVerified = patient.IsEmailVerified;
            patientAccountViewModel.Email = patient.p_Email;
            return patientAccountViewModel;
        }

        public PatientPasswordChangeViewModel PasswordModelTransfer(int id, string Password)
        {
            PatientPasswordChangeViewModel model = new PatientPasswordChangeViewModel();
            model.id = id;
            model.OldPassword = Password;
            return model;
        }


        public BookAppointmentViewModel BookAppointmentModelTransfer(int id)
        {
            Appointment appointment = new Appointment();
            BookAppointmentViewModel model = new BookAppointmentViewModel();
            model.Id = id;
            model.AppointmentDate = appointment.AppointmentDate.Date;            
            model.ProblemDescription = appointment.ProblemDescription;
            model.doctors = context.Doctors.ToList();
            model.departments = context.Departments.ToList();
            model.DepartmentSelectedId = 0;
            model.selectDepartmentList = new SelectList(model.departments, "Id", "DepartmentName", model.DepartmentSelectedId);
            return model;
        }


        public void AddAppointmentToDataBase(BookAppointmentViewModel model)
        {
            Appointment appointment = new Appointment();
            appointment.AppointmentDate = model.AppointmentDate.Add(model.AppointmentTime.TimeOfDay);
            appointment.DoctorId = model.DoctorSelectedId;
            appointment.ProblemDescription = model.ProblemDescription;
            appointment.PatientId = model.Id;
            appointment.DepartmentId = model.DepartmentSelectedId;
            appointment.IsAppointmentActive = true;
            context.Appointments.Add(appointment);
            context.SaveChanges();
        }


        public FindDoctorViewModel FindDoctorModelTansfer(FindDoctorViewModel model, int? departmentSelectedId, int patientId, string DrSearch)
        {
            model.id = patientId;
            model.departmentName = context.Departments.ToList();
            var dept = context.Departments.Where(q => q.Id == departmentSelectedId).FirstOrDefault();
            var doctors = context.Doctors.AsQueryable();
            if ((dept == null) && String.IsNullOrWhiteSpace(DrSearch))
            {
                model.doctorsName = context.Doctors.ToList();
            }
            else if (dept != null && String.IsNullOrWhiteSpace(DrSearch))
            {
                model.doctorsName = dept.Doctors;

            }
            else if (!String.IsNullOrWhiteSpace(DrSearch) && (dept == null))
            {
                model.doctorsName = context.Doctors.Where(q => q.D_UserName.ToUpper().Contains(DrSearch.ToUpper()));
            }
            else if (!String.IsNullOrWhiteSpace(DrSearch) && (dept != null))
            {
                model.doctorsName = dept.Doctors.Where(q => q.D_UserName.ToUpper().Contains(DrSearch.ToUpper()));
            }

            model.DepartmentSelectedId = departmentSelectedId.HasValue ? (int)departmentSelectedId : 0;
            model.selectedDepartment = new SelectList(model.departmentName, "Id", "DepartmentName", model.DepartmentSelectedId);

            return model;
        }
    }
}