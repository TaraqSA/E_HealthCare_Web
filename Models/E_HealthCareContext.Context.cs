﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class E_HealthCareEntities : DbContext
    {
        public E_HealthCareEntities()
            : base("name=E_HealthCareEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<SiteUser> SiteUsers { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<UrlLoggin> UrlLoggins { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }

        public System.Data.Entity.DbSet<E_HealthCare_Web.ViewModels.PatientHomeViewModel> PatientHomeViewModels { get; set; }

        public System.Data.Entity.DbSet<E_HealthCare_Web.ViewModels.EditViewModel> EditViewModels { get; set; }
    }
}
