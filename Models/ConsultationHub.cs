using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using E_HealthCare_Web.Models;
using System.Web.Security;


namespace E_HealthCare_Web
{
    [Authorize]
    public class ConsultationHub : Hub
    {
        E_HealthCareEntities dbContext = new E_HealthCareEntities();        
        public void Consult(string message)
        {   
            Clients.Caller.addNewMessageToPage(message);            
        }


    

        public void sendChatMessage(string doctorUserName, string message)
        {
            //var name = Context.User.Identity.Name; //name of sender client 
            //using (dbContext)
            //{
            //    var User = dbContext.SiteUsers.Where(q => q.UserName == doctorUserName).FirstOrDefault();// returning user who would recieve the message
            //    if(User == null)
            //    {
            //        Clients.Caller.showErrorMessage("Could Not Find That User.");
            //    }
            //    else
            //    {
            //        Clients.User(doctorUserName).addChatMessage(message);

            //        //getting chatConnectionDetails details Connected column as true
            //        var UserToRecieveMessage = dbContext.Entry(User).Collection(u => u.ChatConnectionDetails).Query().Where(q => q.IsConnected == true).FirstOrDefault();
            //        //if(UserToRecieveMessage == null)
            //        //{
            //        //    Clients.Caller.showErrorMessage("The user is no longer connected.");
            //        //}
            //        //else
            //        //{
            //        //    foreach(var connection in User.ChatConnectionDetails)
            //        //    {
            //        //        Clients.Client(connection.SignalRConnectionId).addChatMessage(name,message);
            //        //    }
            //        //}

            //    }
            //}
            
            Clients.User(doctorUserName).sendMessageToDoctor(message);
            Clients.Caller.addNewMessageToPage(message);
            var s = GlobalHost.ConnectionManager.GetHubContext<ConsultationHub>();
            var m = s.Clients.User(doctorUserName);
        }


        public override Task OnConnected()
        {   
            var name = Context.User.Identity.Name;            
            using (dbContext)
            {
                var user = dbContext.SiteUsers.Include(q => q.ChatConnectionDetails).SingleOrDefault(q => q.UserName == name);               
                if(user.ChatConnectionDetails == null || user.ChatConnectionDetails.Count == 0)
                {
                    user.ChatConnectionDetails.Add(new ChatConnectionDetail
                    {
                        SignalRConnectionId = Context.ConnectionId,
                        UserAgent = Context.Request.Headers["User-Agent"],
                        IsConnected = true
                    });
                }
                else
                {
                    var updateUserConnection = user.ChatConnectionDetails.FirstOrDefault();
                    updateUserConnection.IsConnected = true;
                    updateUserConnection.SignalRConnectionId = Context.ConnectionId;
                    updateUserConnection.UserAgent = Context.Request.Headers["User-Agent"];
                }
               
                dbContext.SaveChanges();
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            using (dbContext)
            {
                var chatConnection = dbContext.ChatConnectionDetails.Where(q => q.SignalRConnectionId == Context.ConnectionId).FirstOrDefault();
                chatConnection.IsConnected = false;
                dbContext.SaveChanges();
            }
            return base.OnDisconnected(stopCalled);
        }

    }

    

    

}