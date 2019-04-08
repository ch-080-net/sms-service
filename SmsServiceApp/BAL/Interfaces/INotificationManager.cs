using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;
using System.Threading.Tasks;
using WebApp.Models;

namespace BAL.Managers
{
    public interface INotificationManager
    {
        IEnumerable<EmailNotificationDTO> GetAllEmailNotifications();

        IEnumerable<SmsNotificationDTO> GetAllSmsNotifications();

        IEnumerable<NotificationDTO> GetNewWebNotifications();

        IEnumerable<WebNotificationDTO> GetWebNotificationsPage(string userId, int number);

        NotificationReportDTO GetWebNotificationsReport(string userId);

        TransactionResultDTO AddNotificationsToUser(string userId, DateTime time, string title, string message);

        void SetAsSent(IEnumerable<NotificationDTO> notifications);
        void SetAsSent(NotificationDTO notification);
        void SetAsSent(int notificationId, NotificationOrigin origin, string userId);

    }
}
