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

        void SetAsSent(IEnumerable<NotificationDTO> notifications);
        void SetAsSent(NotificationDTO notification);
    }
}
