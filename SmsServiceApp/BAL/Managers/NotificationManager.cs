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
    /// <summary>
    /// Manager for work with personal and campaign notifications
    /// </summary>
    public class NotificationManager : BaseManager, INotificationManager
    {
        public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Gets all not sent personal and campaign email notifications with valid time
        /// </summary>
        /// <returns>email messages</returns>
        public IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            return GetAllPersonalEmailNotifications().Concat(GetAllCampaignEmailNotifications());
        }

        /// <summary>
        /// Gets all not sent personal email notifications with valid time
        /// </summary>
        /// <returns>email messages</returns>
        private IEnumerable<EmailNotificationDTO> GetAllPersonalEmailNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Email);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }

        /// <summary>
        /// Gets all not sent campaign email notifications based on campaign events time
        /// </summary>
        /// <returns>email messages</returns>
        private IEnumerable<EmailNotificationDTO> GetAllCampaignEmailNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Email
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<EmailNotificationDTO>>(notifications);
            return result;
        }


        /// <summary>
        /// Gets all not sent personal and campaign SMS notifications with valid time
        /// </summary>
        /// <returns>SMS messages</returns>
        public IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            return GetAllPersonalSmsNotifications().Concat(GetAllCampaignSmsNotifications());
        }

        /// <summary>
        /// Gets all not sent personal SMS notifications with valid time
        /// </summary>
        /// <returns>SMS messages</returns>
        private IEnumerable<SmsNotificationDTO> GetAllPersonalSmsNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Sms);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<SmsNotificationDTO>>(notifications);
            return result;
        }

        /// <summary>
        /// Gets all not sent campaign SMS notifications based on campaign events time
        /// </summary>
        /// <returns>SMS messages</returns>
        private IEnumerable<SmsNotificationDTO> GetAllCampaignSmsNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Sms
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<SmsNotificationDTO>>(notifications);
            return result;
        }


        /// <summary>
        /// Gets all not sent personal and campaign web notifications with valid time
        /// </summary>
        /// <returns>Web notifications</returns>
        public IEnumerable<WebNotificationDTO> GetAllWebNotifications()
        {
            return GetAllPersonalWebNotifications().Concat(GetAllCampaignWebNotifications());
        }

        /// <summary>
        /// Gets all not sent personal web notifications with valid time
        /// </summary>
        /// <returns>Web notifications</returns>
        private IEnumerable<WebNotificationDTO> GetAllPersonalWebNotifications()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.Now
                                                                && n.Type == NotificationType.Web);
            var result = mapper.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>(notifications);
            return result;
        }

        /// <summary>
        /// Gets all not sent campaign web notifications based on campaign events time
        /// </summary>
        /// <returns>Web notifications</returns>
        private IEnumerable<WebNotificationDTO> GetAllCampaignWebNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Web
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>(notifications);
            return result;
        }

        /// <summary>
        /// Set enumeration of NotificationDTO as sent in Notifications and CampaignNotifications tables
        /// </summary>
        public void SetAsSent(IEnumerable<NotificationDTO> notifications)
        {
            foreach (var notification in notifications)
            {
                SetAsSent(notification);
            }
        }

        /// <summary>
        /// Set NotificationDTO as sent in Notifications or CampaignNotifications table
        /// </summary>
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

        /// <summary>
        /// Set notificaton as sent in corresponding tables
        /// </summary>
        /// <param name="notificationId">id of notification to set</param>
        /// <param name="origin">table with notification</param>
        /// <param name="userId">Id of ApplicationUser who owns notification</param>
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
