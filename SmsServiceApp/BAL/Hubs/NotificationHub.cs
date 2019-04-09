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
        /// Sets notification with given Id and origin as sent
        /// </summary>
        public void ConfirmReceival(int notificationId, NotificationOrigin origin)
        {
            notificationManager.SetAsSent(notificationId, origin, Context.UserIdentifier);
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
    }
}
