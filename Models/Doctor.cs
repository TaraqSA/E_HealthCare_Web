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
    
    public partial class Doctor
    {
        public int Id { get; set; }
        public string D_Name { get; set; }
        public string D_Email { get; set; }
        public Nullable<System.DateTime> D_DateOfBirth { get; set; }
        public string D_UserName { get; set; }
        public Nullable<int> D_UserId { get; set; }
    
        public virtual SiteUser SiteUser { get; set; }
    }
}