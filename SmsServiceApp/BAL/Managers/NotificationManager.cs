using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using Model.Interfaces;
using AutoMapper;
using System.Linq;
using Model.DTOs;

namespace BAL.Managers
{
    public class NotificationManager : BaseManager, INotificationManager
    {
        public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.UtcNow
                                                                && n.Type == NotificationType.Email);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }

        public void SetAsSent(IEnumerable<NotificationDTO> notifications)
        {
            var nots = unitOfWork.Notifications.Get(n => notifications.Any(ndto => ndto.Id == n.Id));
            foreach(var iter in nots)
            {
                iter.BeenSent = true;
            }

            try
            {
                unitOfWork.Save();
            }
            catch
            {
                // Sending will be repeated
            }
        }

        public void SetAsSent(NotificationDTO notification)
        {
            var not = unitOfWork.Notifications.GetById(notification.Id);
            if (not != null)
            {
                not.BeenSent = true;
            }

            try
            {
                unitOfWork.Save();
            }
            catch
            {
                // Sending will be repeated
            }
        }
    }
}
