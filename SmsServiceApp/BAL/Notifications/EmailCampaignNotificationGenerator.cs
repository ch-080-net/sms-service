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

namespace BAL.Notifications
{
    public class EmailCampaignNotificationGenerator : INotificationsGenerator<EmailCampaign>
    {
        private readonly IUnitOfWork unitOfWork;
        public EmailCampaignNotificationGenerator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmailCampaign SupplyWithCampaignNotifications(EmailCampaign item)
        {
            if (item == null)
            {
                return item;
            }
            ApplicationUser user;
            if (item.User != null)
            {
                user = item.User;
            }
            else
            {
                user = unitOfWork.ApplicationUsers.Get(x => x.Id == item.UserId).FirstOrDefault();
                if (user == null)
                    return item;
            }

            AddSpecificNotifications(user, item, NotificationType.Web);
            if (user.EmailNotificationsEnabled && user.EmailConfirmed)
            {
                AddSpecificNotifications(user, item, NotificationType.Email);
            }
            if (user.SmsNotificationsEnabled && user.PhoneNumberConfirmed)
            {
                AddSpecificNotifications(user, item, NotificationType.Sms);
            }
            return item;
        }

        private void AddSpecificNotifications(ApplicationUser user, EmailCampaign campaign, NotificationType type)
        {
            if (campaign.EmailCampaignNotifications == null)
                campaign.EmailCampaignNotifications = new List<EmailCampaignNotification>();

            campaign.EmailCampaignNotifications.Add(new EmailCampaignNotification()
            {
                BeenSent = false,
                Type = type,
                ApplicationUserId = user.Id
            });
        }
    }
}
