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
            unitOfWork.Save();
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
            unitOfWork.Save();
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
        /// <param name="userId">Identity user Id</param>
        /// <param name="time">Time, when messages should be sent</param>
        /// <param name="title">Title of message</param>
        /// <param name="message">Text of message</param>
        /// <param name="href">Optional hyper reference</param>
        /// <returns>succes result if succesfull</returns>
        public TransactionResultDTO AddNotificationsToUser(string userId, DateTime time, string title, string message, string href = null)
        {
            var user = unitOfWork.ApplicationUsers.Get(au => au.Id == userId).FirstOrDefault();
            if (user == null)
                return new TransactionResultDTO() { Success = false, Details = "User does not exist!" };

            AddSpecificNotifications(userId, NotificationType.Web, time, title, message, href);
            if (user.EmailNotificationsEnabled && user.EmailConfirmed)
            {
                AddSpecificNotifications(userId, NotificationType.Email, time, title, message, href);
            }
            if (user.SmsNotificationsEnabled && user.PhoneNumberConfirmed)
            {
                AddSpecificNotifications(userId, NotificationType.Sms, time, title, message, href);
            }
            try
            {
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }

            return new TransactionResultDTO() { Success = true };
        }

        private void AddSpecificNotifications(string userId, NotificationType type, DateTime time, string title, string message, string href = null)
        {            
            unitOfWork.Notifications.Insert(new Notification()
            {
                BeenSent = false,
                Type = type,
                Title = title,
                Message = message,
                Time = time,
                ApplicationUserId = userId,
                Href = href                
            });       
        }

        public int GetNumberOfWebNotifications(string userId)
        {
            return handler.GetNumOfWebNotifications(userId);
        }
    }
}
