using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_HealthCare_Web.Models;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace E_HealthCare_Web.ViewModels
{
    public class AdminHomeViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Full Name", Description = "Admin Name")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string AdminName { get; set; }

        public string ProfileImagePath { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, NullDisplayText = "Not Filled", DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Email")]
        [DisplayFormat(NullDisplayText = "Not filled")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        public string Gender { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        public string Address { get; set; }

        [DisplayFormat(NullDisplayText = "Not filled")]
        [Display(Name = "Contact")]
        public string Phone { get; set; }

        [DisplayFormat(NullDisplayText ="No Doctors")]
        [Display(Name ="Total Doctors")]
        public int DoctorList { get; set; }

        [DisplayFormat(NullDisplayText = "No Patients")]
        [Display(Name = "Total Patients")]
        public int PatientList { get; set; }

        [DisplayFormat(NullDisplayText = "No Departments")]
        [Display(Name = "Total Departments")]
        public int DepartmentList { get; set; }
    }

    public class AdminEditViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Full Name", Description = "Admin Name")]
        [Required(ErrorMessage = "enter admin name")]
        public string AdminName { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Enter Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Date of Birth required")]
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

    public class AdminProfileViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
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

    public class AdminAccountViewModel
    {
        public int id { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public bool IsEmailVerified { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class AdminPasswordChangeViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "field is required")]
        [DataType(DataType.Password)]
        [Remote("IsCorrectPassword", "Admin", ErrorMessage = "Password is incorrect", AdditionalFields = "id")]
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
    public class AdminListViewModel
    {
        [Key]
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }

        [DisplayFormat(NullDisplayText = "Unavailable")]
        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "Unavailable")]
        public string Gender { get; set; }

        [Display(Name = "Contact")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(NullDisplayText = "Unavailable")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class CreateAdminViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Admin Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "name is mandatory")]
        public string AdminName { get; set; }

        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "email is mandatory")]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "email is in use")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "username is mandatory")]
        [Remote("IsUserNameAlreadyExist", "Account", ErrorMessage = "username already exists")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is mandatory")]
        [DataType(DataType.Password)]        
        [Remote("IsPasswordValid", "Account", ErrorMessage = "Password Must be Atleast 6 Characters Long with atleast one upper letter and a number")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is mandatory")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "password does not match")]
        public string ConfirmPassword { get; set; }
    }


    public class DoctorsListViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DisplayFormat(NullDisplayText = "Unavailable")]
        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "Unavailable")]
        public string Gender { get; set; }

        [Display(Name = "Contact")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(NullDisplayText = "Unavailable")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
    
    public class DepartmentListViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }
}