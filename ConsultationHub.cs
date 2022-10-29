using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_HealthCare_Web
{
    public class ConsultationHub : Hub
    {
        public void Consult(string name, string message)
        {
            Clients.All.addNewMessageToPage(name,message);
            
        }
    }
}