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
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public Nullable<int> PatientAge { get; set; }
        public Nullable<int> PatientGenderId { get; set; }
        public string PatientEmail { get; set; }
        public string PatientAddress { get; set; }
    }
}
