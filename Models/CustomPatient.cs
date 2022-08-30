using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace E_HealthCare_Web.Models
{
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {
    }

    public class PatientMetadata
    {
        [Display(Name = "Full Name")]
        [DisplayFormat(NullDisplayText ="Not Filled")]
        public string p_name { get; set; }


        [Display(Name = "Gender")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string p_gender { get; set; }

        [Display(Name = "Email")]        
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Please Enter a valid Email Address")]
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "Email Address is already in use")]
        public string p_Email { get; set; }

        [Display(Name = "Date Of Birth")]        
        [DataType(DataType.Date,ErrorMessage ="Please Enter a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true ,DataFormatString ="{0:yyyy-MM-dd}", NullDisplayText ="Not Filled")]
        public Nullable<System.DateTime> p_dateOfBirth { get; set; }

        [Display(Name = "Address")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string p_address { get; set; }

        [Display(Name = "Contact")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Enter Valid Phone Number")]        
        public string p_phone { get; set; }

        [Display(Name = "Blood Group")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string p_BloodGroup { get; set; }

        [Display(Name = "User Name")]        
        public string UserName { get; set; }
    }


}