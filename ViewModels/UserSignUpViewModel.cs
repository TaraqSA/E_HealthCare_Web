using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;


namespace E_HealthCare_Web.ViewModels
{
    public class UserSignUpViewModel
    {

        [Required(ErrorMessage ="Please Enter User Name")]
        [Display(Name ="User Name")]
        [Remote("IsUserNameAlreadyExist", "Account", ErrorMessage = "UserName Already Exist")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Full Name")]
        [Display(Name ="Full Name")]
        public string PatientName { get; set; }

        
        [Display(Name ="Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,  DataFormatString = "{0:dd/mm/yyyy}")]          
        public DateTime DateOfBirth { get; set; }

        [Display(Name ="Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name ="Email")]
       // [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        [DataType(DataType.EmailAddress ,ErrorMessage = "please enter a valid email")]
        [EmailAddress(ErrorMessage = "please enter a valid email")]
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage ="Email Address is already in use")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name ="Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name ="Password")]
        [StringLength(100, ErrorMessage ="The {0} must be atleast {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Please enter password once again to confirm")]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and Confrim Password not matching")]        
        public string ConfirmPassword { get; set; }
               
    }
}