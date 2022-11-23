using E_HealthCare_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace E_HealthCare_Web.ViewModels
{
    public class DoctorHomeViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Full Name", Description = "Doctor Name")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string DoctorName { get; set; }

        public string ProfileImagePath { get; set; }

        [DisplayFormat(NullDisplayText = "None")]
        public IEnumerable<Department> Speciality { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, NullDisplayText = "Not Filled", DataFormatString ="{0:dd-MM-yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Email")]
        [DisplayFormat(NullDisplayText = "Not filled")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DisplayFormat(NullDisplayText ="Not filled")]        
        public string Gender { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        [Display(Name ="Blood Group")]
        public string BloodGroup { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        public string Address { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        [Display(Name = "Contact")]
        public string Phone { get; set; }

        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Appointment Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime AppointmentTime { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }

    public class EditDoctorViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Full Name", Description = "Doctor Name")]        
        [Required(ErrorMessage = "enter doctor name")]
        public string DoctorName { get; set; }               

        //[DisplayFormat(NullDisplayText = "None")]
        //public IEnumerable<Department> Speciality { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Enter Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage ="Date of Birth required")]     
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "enter gender")]
        public string Gender { get; set; }
                
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        public string Address { get; set; }

        [Display(Name = "Contact No")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Valid Phone Number")]
        public string Phone { get; set; }
    }
    
    public class DoctorProfileViewModel
    {
        [Key]
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Name { get; set; }

        [Display(Name = "Gender")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Gender { get; set; }

        [Display(Name = "Email")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a valid Email Address")]        
        public string Email { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date, ErrorMessage = "Please Enter a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}", NullDisplayText = "Not Filled")]
        public Nullable<System.DateTime> DOB { get; set; }

        [Display(Name = "Address")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Address { get; set; }

        [Display(Name = "Contact")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Valid Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Blood Group")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string BloodGroup { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }

    }

    public class DoctorAccountViewModel
    {
        public int id { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public bool IsEmailVerified { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class DoctorPasswordChangeViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "field is required")]
        [DataType(DataType.Password)]
        [Remote("IsCorrectPassword", "Doctor", ErrorMessage = "Password is incorrect", AdditionalFields = "id")]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "field is required")]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "field is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Does Not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }
    }

    public class DoctorAppointmentListViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Patient Name")]
        [DisplayFormat(NullDisplayText = "Not Available")]
        public string patientName { get; set; }

        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime appointmentDate { get; set; }

        [Display(Name = "Appointment Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime appointmentTime { get; set; }

        [Display(Name = "Department")]
        public string departmentName { get; set; }
    }
}