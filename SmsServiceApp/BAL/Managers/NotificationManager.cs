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

namespace BAL.Managers
{
    public class NotificationManager : BaseManager, INotificationManager
    {
        UserManager<ApplicationUser> userManager;
        public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : base(unitOfWork, mapper)
        {
            this.userManager = userManager;
        }

        public IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.UtcNow
                                                                && n.Type == NotificationType.Email);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }

        public IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.UtcNow
                                                                && n.Type == NotificationType.Sms);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<SmsNotificationDTO>>(notifications);
            return result;
        }

        public IEnumerable<WebNotificationDTO> GetAllWebNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.UtcNow
                                                                && n.Type == NotificationType.Web);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>(notifications);
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

        public void SetAsSent(int notificationId, NotificationOrigin origin, string userId)
        {
            switch (origin)
            {
                case NotificationOrigin.PersonalNotification:
                    {
                        var not = unitOfWork.Notifications.Get(x => x.Id == notificationId && x.ApplicationUserId == userId).FirstOrDefault();
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
                        break;
                    }

                case NotificationOrigin.CampaignReport:
                    {
                        var not = unitOfWork.CampaignNotification.Get(x => x.Id == notificationId && x.ApplicationUserId == userId).FirstOrDefault();
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
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
        }
    }
}
