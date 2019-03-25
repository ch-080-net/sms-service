using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BAL.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification()
        {
            await Clients.All.SendAsync("GetNotification", "Hi!");
        }
    }
}
