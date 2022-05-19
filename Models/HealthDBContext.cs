using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace E_HealthCare_Web.Models
{
    public class HealthDBContext : DbContext 
    {
      //  public DbSet<doctors> doctors { get; set; }
        public DbSet<LoginDetails> loginDetails { get; set; }
    }
}