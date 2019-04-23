using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BAL.Notifications.Infrastructure;
using Model.DTOs;
using Model.Interfaces;
using WebApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BAL.Notifications
{
    public class PersonalNotificationHandler : INotificationHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PersonalNotificationHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Email);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }

        public IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Sms);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<SmsNotificationDTO>>(notifications);
            return result;
        }

        public int GetNumOfWebNotifications(string userId)
        {
            int result = unitOfWork.Notifications.Get(n => n.Time <= DateTime.Now
                && n.Type == NotificationType.Web && n.ApplicationUserId == userId && !n.BeenSent).Count();
            return result;
        }

        public IEnumerable<WebNotificationDTO> GetWebNotifications(string userId, int quantity = 5)
        {
            quantity = (quantity < 1) ? 5 : quantity;
            var notifications = unitOfWork.Notifications
                .Get(n => n.Time <= DateTime.Now && n.Type == NotificationType.Web && n.ApplicationUserId == userId)
                .OrderByDescending(x => x.Time).Take(quantity);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>(notifications);
            return result;
        }

        public NotificationReportDTO GetWebNotificationsReport(string userId)
        {
            var result = new NotificationReportDTO();
            result.Notifications = GetWebNotifications(userId);
            return result;
        }

        public void SetAsSent(IEnumerable<NotificationDTO> notifications)
        {
            if (notifications == null || !notifications.Any())
                return;
            var actualNotifications = notifications.Where(x => x.Origin == NotificationOrigin.PersonalNotification);
            var personalNotifications = unitOfWork.Notifications.Get(x => actualNotifications.Any(y => y.Id == x.Id));
            foreach (var notification in personalNotifications)
            {
                notification.BeenSent = true;
            }
        }

        public void SetAsSent(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;
            var personalNotifications = unitOfWork.Notifications.Get(n => n.ApplicationUserId == userId);
            foreach (var notification in personalNotifications)
            {
                notification.BeenSent = true;
            }
        }
    }
}
