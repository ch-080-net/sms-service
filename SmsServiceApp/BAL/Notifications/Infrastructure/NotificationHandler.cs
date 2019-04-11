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
    public abstract class NotificationHandler : INotificationHandler
    {
        protected readonly INotificationHandler notificationHandler;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        public NotificationHandler(INotificationHandler notificationHandler, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.notificationHandler = notificationHandler;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public abstract IEnumerable<EmailNotificationDTO> GetAllEmailNotifications();

        public abstract IEnumerable<SmsNotificationDTO> GetAllSmsNotifications();

        public abstract IEnumerable<WebNotificationDTO> GetWebNotifications(string UserId, int quantity = 5);

        public abstract NotificationReportDTO GetWebNotificationsReport(string userId);

        public abstract void SetAsSent(IEnumerable<NotificationDTO> notifications);

        public abstract void SetAsSent(int notificationId, NotificationOrigin origin, string userId);
    }
}
