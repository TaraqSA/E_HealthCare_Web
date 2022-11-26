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
        ConversationInfo messageInfo = new ConversationInfo();

        public void Consult(string message)
        {
            Clients.Caller.addNewMessageToPage(message);
        }

        public void sendChatMessage(string ToUserName, string message)
        {
            using (dbContext)
            {
                var ToUser = dbContext.SiteUsers.Where(q => q.UserName == ToUserName).FirstOrDefault();
                messageInfo.MessageDate = DateTime.Now;
                messageInfo.MessageText = message;
                messageInfo.SenderId = dbContext.SiteUsers.Where(q => q.UserName == Context.User.Identity.Name).Select(q => q.Id).FirstOrDefault();
                messageInfo.RecieverId = ToUser.Id;
                dbContext.ConversationInfoes.Add(messageInfo);
                dbContext.SaveChanges();

                if (ToUser.UserRole == "Doctor")
                {
                    Clients.User(ToUserName).sendMessageToPatient(message);
                    Clients.Caller.addNewMessageToPage(message);
                }
                if (ToUser.UserRole == "Patient")
                {
                    Clients.User(ToUserName).sendMessageToDoctor(message);
                    Clients.Caller.addNewMessageToPage(message);
                }
            }
        }


        public override Task OnConnected()
        {
            var name = Context.User.Identity.Name;
            using (dbContext)
            {
                var user = dbContext.SiteUsers.Include(q => q.ChatConnectionDetails).SingleOrDefault(q => q.UserName == name);
                if (user.ChatConnectionDetails == null || user.ChatConnectionDetails.Count == 0)
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