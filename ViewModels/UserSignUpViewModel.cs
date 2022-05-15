using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace E_HealthCare_Web.ViewModels
{
    public class UserSignUpViewModel
    {

        [Required(ErrorMessage ="*")]
        [Display(Name ="UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name ="FullName")]
        public string PatientName { get; set; }

        
        [Display(Name ="Age")]
        public Nullable<int> PatientAge { get; set; }

        [Display(Name ="Gender")]
        public Nullable<int> PatientGenderId { get; set; }

        [Required(ErrorMessage = "Email id is not Valid")]
        [Display(Name ="Email")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string PatientEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name ="Address")]
        public string PatientAddress { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string LoginPassWord { get; set; }
    }
}