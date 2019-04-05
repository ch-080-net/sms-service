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
        private readonly INotificationManager notificationManager;
        public NotificationHub(INotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }

        public void ConfirmReceival(int notificationId, NotificationOrigin origin)
        {
            notificationManager.SetAsSent(notificationId, origin, Context.UserIdentifier);
        }

        public IEnumerable<WebNotificationDTO> GetNotificationPage(int number)
        {
            return notificationManager.GetWebNotificationsPage(Context.UserIdentifier, number);
        }
    }
}
