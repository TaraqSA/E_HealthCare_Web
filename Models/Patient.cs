//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_HealthCare_Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patient
    {
        public int p_id { get; set; }
        public string p_name { get; set; }
        public string p_gender { get; set; }
        public string p_Email { get; set; }
        public Nullable<System.DateTime> p_dateOfBirth { get; set; }
        public string p_address { get; set; }
        public string p_phone { get; set; }
        public string p_BloodGroup { get; set; }
        public string resetCode { get; set; }
        public string UserName { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual SiteUser SiteUser { get; set; }
    }
}
