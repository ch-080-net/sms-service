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
            return GetAllPersonalEmailNotifications().Concat(GetAllCampaignEmailNotifications());
        }

        private IEnumerable<EmailNotificationDTO> GetAllPersonalEmailNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Email);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }

        private IEnumerable<EmailNotificationDTO> GetAllCampaignEmailNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == CampaignNotificationType.Email
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }



        public IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            return GetAllPersonalSmsNotifications().Concat(GetAllCampaignSmsNotifications());
        }

        private IEnumerable<SmsNotificationDTO> GetAllPersonalSmsNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Sms);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<SmsNotificationDTO>>(notifications);
            return result;
        }

        private IEnumerable<SmsNotificationDTO> GetAllCampaignSmsNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == CampaignNotificationType.Sms
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<SmsNotificationDTO>>(notifications);
            return result;
        }



        public IEnumerable<WebNotificationDTO> GetAllWebNotifications()
        {
            return GetAllPersonalWebNotifications().Concat(GetAllCampaignWebNotifications());
        }

        private IEnumerable<WebNotificationDTO> GetAllPersonalWebNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Web);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>(notifications);
            return result;
        }

        private IEnumerable<WebNotificationDTO> GetAllCampaignWebNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == CampaignNotificationType.Web
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>(notifications);
            return result;
        }

        public void SetAsSent(IEnumerable<NotificationDTO> notifications)
        {
            foreach (var notification in notifications)
            {
                SetAsSent(notification);
            }
        }

        public void SetAsSent(NotificationDTO notification)
        {
            switch(notification.Origin)
            {
                case NotificationOrigin.PersonalNotification:
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
                        break;
                    }
                case NotificationOrigin.CampaignReport:
                    {
                        var not = unitOfWork.CampaignNotifications.GetById(notification.Id);
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
                        var not = unitOfWork.CampaignNotifications.Get(x => x.Id == notificationId && x.ApplicationUserId == userId).FirstOrDefault();
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
