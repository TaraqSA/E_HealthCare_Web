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
    
    public partial class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string ProfileImagePath { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public bool IsEmailVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string ResetCode { get; set; }
    
        public virtual SiteUser SiteUser { get; set; }
    }
}
