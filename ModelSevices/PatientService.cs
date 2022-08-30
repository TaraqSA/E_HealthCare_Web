using E_HealthCare_Web.Models;
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
            if(getPatient.p_gender != null && getPatient.p_gender.Length == 1)
            {
                patientHomeViewModel.Gender =  ChangeGenderValue(getPatient.p_gender);
            }
            return patientHomeViewModel;
        }

        public void UpdatePatient(Patient getPatient,EditViewModel model)
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
        
    }
}