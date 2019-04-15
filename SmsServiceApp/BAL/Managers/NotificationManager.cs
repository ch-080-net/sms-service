using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using Model.Interfaces;
using AutoMapper;
using System.Linq;
using Model.DTOs;
using System.Threading.Tasks;
using BAL.Notifications;
using BAL.Notifications.Infrastructure;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for work with personal and campaign notifications
    /// </summary>
    public class NotificationManager : BaseManager, INotificationManager
    {
        private readonly INotificationHandler handler;

        public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            handler = new PersonalNotificationHandler(unitOfWork, mapper);
            handler = new EmailCampaignNotificationsHandler(handler, unitOfWork, mapper);
            handler = new SmsCampaignNotificationHandler(handler, unitOfWork, mapper);
        }

        /// <summary>
        /// Gets all not sent personal and campaign email notifications with valid time
        /// </summary>
        /// <returns>email messages</returns>
        public IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            var result = handler.GetAllEmailNotifications();
            return result;
        }

        /// <summary>
        /// Gets all not sent personal and campaign SMS notifications with valid time
        /// </summary>
        /// <returns>SMS messages</returns>
        public IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            var result = handler.GetAllSmsNotifications();
            return result;           
        }     
      

        /// <summary>
        /// Set enumeration of NotificationDTO as sent in Notifications and CampaignNotifications tables
        /// </summary>
        public void SetAsSent(IEnumerable<NotificationDTO> notifications)
        {
            handler.SetAsSent(notifications);
            try
            {
                unitOfWork.Save();
            }
            catch
            {
                // Notification sending will be repeated
            }
    }      

        /// <summary>
        /// Set notificaton as sent in corresponding tables
        /// </summary>
        /// <param name="notificationId">id of notification to set</param>
        /// <param name="origin">table with notification</param>
        /// <param name="userId">Id of ApplicationUser who owns notification</param>
        public void SetAsSent(string userId)
        {

            handler.SetAsSent(userId);
            try
            {
                unitOfWork.Save();
            }
            catch
            {
                // Notification badge will be filled
            }
        }


        /// <summary>
        /// Generate list of sent notifications in descending order for User with userId
        /// </summary>
        /// <param name="userId">Identity User Id</param>
        /// <param name="number">Quantity of returned notifications</param>
        /// <returns>Enumeration of notifications in descending (by time) order</returns>
        public IEnumerable<WebNotificationDTO> GetWebNotificationsPage(string userId, int number)
        {
            var result = handler.GetWebNotifications(userId, number);
            return result;
        }

        /// <summary>
        /// Generate web notifications report class with latest notifications and statistics
        /// </summary>
        /// <param name="userId">Application User Id</param>
        public NotificationReportDTO GetWebNotificationsReport(string userId)
        {
            var result = handler.GetWebNotificationsReport(userId);
            return result;
        }

        /// <summary>
        /// Add personal notification to Identity User
        /// </summary>
        /// <returns>succes result if succesfull</returns>
        public TransactionResultDTO AddNotificationsToUser(ICollection<Notification> notifications)
        {
            try
            {
                foreach (var notification in notifications)
                {
                    unitOfWork.Notifications.Insert(notification);
                }
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO { Success = false, Details = "Notification addition failed" };
            }
            return new TransactionResultDTO { Success = true };
        }

        public int GetNumberOfWebNotifications(string userId)
        {
            return handler.GetNumOfWebNotifications(userId);
        }
    }
}
