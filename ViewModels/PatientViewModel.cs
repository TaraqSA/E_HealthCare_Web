using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace E_HealthCare_Web.ViewModels
{

    public class PatientHomeViewModel
    {   
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
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "Email Address is already in use")]
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
    public class EditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Required")]
        public string Gender { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Enter Valid EmailAddress")]
        [Display(Name = "Email Address")]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Enter date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:yyyy-MM-dd}", NullDisplayText = "Not Filled")]
        [Display(Name ="Date Of Birth")]
        [Required]
        public Nullable<System.DateTime> DOB { get; set; }
        
        [Display(Name ="Address")]
        public string Address { get; set; }

        [Display(Name ="Phone Number")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Enter Phone Number")]
        public string Phone { get; set; }

        [Display(Name ="Blood Group")]
        [DataType(DataType.Text)]
        public string BloodGroup { get; set; }        
    }

    public class PatientProfileViewModel
    {
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
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "Email Address is already in use")]
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

    public class PatientAccountViewModel
    {
        public int id { get; set; }

        [Display(Name ="UserName")]
        public string UserName { get; set; }

        public bool IsEmailVerified { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class PatientPasswordChangeViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage ="field is required")]
        [DataType(DataType.Password)]
        [Remote("IsCorrectPassword", "Patient", ErrorMessage ="Password is incorrect",AdditionalFields ="id")]
        [Display(Name ="Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage ="field is required")]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="field is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Password Does Not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }
    }
   
}