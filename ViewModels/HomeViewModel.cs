using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_HealthCare_Web.ViewModels
{
    public class AskQuestionViewModel
    {
        [Required(ErrorMessage ="Please Enter your Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="Please Enter Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Enter Your Contact Number")]
        public string Contact { get; set; }

        [Required]
        public bool PrivacyPermission { get; set; }
  
    }
    
}