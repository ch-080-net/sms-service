using System;
using System.Collections.Generic;
using System.Text;
using BAL.Notifications.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;

namespace BAL.Notifications
{
    public class PersonalNotificationBuilder : IPersonalNotificationBuilder
    {
        private readonly ApplicationUser applicationUser;
        private ICollection<Notification> notifications;
        public PersonalNotificationBuilder(ApplicationUser applicationUser)
        {
            this.applicationUser = applicationUser;
            notifications = new List<Notification> { new Notification { Type = NotificationType.Web } };
            if (applicationUser.EmailConfirmed && applicationUser.EmailNotificationsEnabled)
                notifications.Add(new Notification { Type = NotificationType.Email });
            if (applicationUser.PhoneNumberConfirmed && applicationUser.SmsNotificationsEnabled)
                notifications.Add(new Notification { Type = NotificationType.Sms });
        }

        public ApplicationUser Build()
        {
            applicationUser.Notifications = applicationUser.Notifications ?? new List<Notification>();
            applicationUser.Notifications = applicationUser.Notifications.Concat(notifications).ToList();
            return applicationUser;
        }

        public IPersonalNotificationBuilder GenerateHref(IUrlHelper urlHelper
            , string controller, string action, object values = null)
        {
            string href = urlHelper.Action(action, controller, values);
            foreach (var notification in notifications)
            {
                notification.Href = href;
            }
            return this;
        }

        public IPersonalNotificationBuilder SetHref(string href)
        {
            foreach (var notification in notifications)
            {
                notification.Href = href;
            }
            return this;
        }

        public IPersonalNotificationBuilder SetMessage(string title, string message)
        {
            foreach(var notification in notifications)
            {
                notification.Title = title;
                notification.Message = message;
            }
            return this;
        }

        public IPersonalNotificationBuilder SetTime(DateTime time)
        {
            foreach(var notification in notifications)
            {
                notification.Time = time;
            }
            return this;
        }
    }
}
