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
       
        IEnumerable<WebNotificationDTO> GetWebNotificationsPage(string userId, int number);

        int GetNumberOfWebNotifications(string userId);

        NotificationReportDTO GetWebNotificationsReport(string userId);

        TransactionResultDTO AddNotificationsToUser(ICollection<Notification> notifications);

        void SetAsSent(IEnumerable<NotificationDTO> notifications);
        void SetAsSent(string userId);

    }
}
