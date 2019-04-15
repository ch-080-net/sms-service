using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using Model.Interfaces;
using AutoMapper;
using System.Linq;
using Model.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BAL.Notifications.Infrastructure
{
    public interface INotificationHandler
    {
        IEnumerable<WebNotificationDTO> GetWebNotifications(string userId, int quantity = 5);
        NotificationReportDTO GetWebNotificationsReport(string userId);
        int GetNumOfWebNotifications(string userId);

        IEnumerable<EmailNotificationDTO> GetAllEmailNotifications();

        IEnumerable<SmsNotificationDTO> GetAllSmsNotifications();

        void SetAsSent(IEnumerable<NotificationDTO> notifications);
        void SetAsSent(string userId);        

    }
}
