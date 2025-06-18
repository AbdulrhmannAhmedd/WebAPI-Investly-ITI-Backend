using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Investly.PL.Hubs
{
    public class NotificationHub : Hub
    {
        //public  async Task SendNotification(string UserId, int Count)
        //{
        //    await Clients.User(UserId).SendAsync("RecieveNotificationCount", Count);
        //}
    }

    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        
        {
            var connection1 = connection;
            var u = connection.User;
            return connection.User?.FindFirst(c=>c.Type=="id")?.Value;
        }
    }
}
