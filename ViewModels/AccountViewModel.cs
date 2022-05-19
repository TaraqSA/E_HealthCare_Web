using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace E_HealthCare_Web.ViewModels
{
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