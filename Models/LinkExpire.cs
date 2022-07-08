using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_HealthCare_Web.Models
{
    public class LinkExpire
    {
        public DateTime ExpiresOn { get; set; }
        public string CreateToken { get; set; }
    }
}