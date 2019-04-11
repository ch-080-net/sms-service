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

        /// <summary>
        /// Gets enumeration of actual notifications for user
        /// </summary>
        /// <param name="number">Maximum quantity of notification</param>
        public IEnumerable<WebNotificationDTO> GetNotificationPage(int number)
        {
            return notificationManager.GetWebNotificationsPage(Context.UserIdentifier, number);
        }

        /// <summary>
        /// Gets NotificationReportDTO for User
        /// </summary>
        public NotificationReportDTO GetNotificationReport()
        {
            return notificationManager.GetWebNotificationsReport(Context.UserIdentifier);
        }

        public int GetNumberOfNotifications()
        {
            return notificationManager.GetNumberOfWebNotifications(Context.UserIdentifier);
        }

        public override async Task OnConnectedAsync()
        {
            var result = notificationManager.GetNumberOfWebNotifications(Context.UserIdentifier);
            await Clients.User(Context.UserIdentifier).SendAsync("NotificationsNumRecieved", result);
        }
    }
}
