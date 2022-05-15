using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace E_HealthCare_Web.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name ="UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}