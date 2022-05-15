using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_HealthCare_Web.Models
{
   [Table("LoginDetails")]
    public class LoginDetails
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string LoginPassWord { get; set; }
    }
}