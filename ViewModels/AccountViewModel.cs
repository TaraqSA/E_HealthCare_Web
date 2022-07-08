using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace E_HealthCare_Web.ViewModels
{
    public class UserSignUpViewModel
    {

        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "User Name")]
        [Remote("IsUserNameAlreadyExist", "Account", ErrorMessage = "UserName Already Exist")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email")]
        // [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        [DataType(DataType.EmailAddress, ErrorMessage = "please enter a valid email")]
        [EmailAddress(ErrorMessage = "please enter a valid email")]
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "Email Address is already in use")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter password once again to confirm")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confrim Password not matching")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "field is mandatory")]
        [Display(Name = "Register as")]
        public string UserRole { get; set; }

    }

    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="please enter email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="Email Address")]        
        public string EmailAddress { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter password", AllowEmptyStrings = false)]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confrim Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetPasswordCode { get; set; }
    }



}