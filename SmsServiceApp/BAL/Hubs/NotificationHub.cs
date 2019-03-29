using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Model.DTOs;
using BAL.Managers;

namespace BAL.Hubs
{
    public class NotificationHub : Hub
    {
        INotificationManager notificationManager;
        public NotificationHub(INotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }

        public async Task ConfirmReceival(int notificationId)
        {
            notificationManager.SetAsSent(notificationId, Context.UserIdentifier);
        }
    }
}
