using System.Collections.Generic;
using WebApp.Models;
using Model.Interfaces;
using BAL.Notifications.Infrastructure;

namespace BAL.Notifications
{
    public class SmsCampaignNotificationGenerator : INotificationsGenerator<Company>
    {
        private readonly IUnitOfWork unitOfWork;
        public SmsCampaignNotificationGenerator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Company SupplyWithCampaignNotifications(Company item)
        {
            if (item == null)
            {
                return item;
            }
            var users = unitOfWork.ApplicationUsers.Get(au => au.ApplicationGroupId == item.ApplicationGroupId);
            foreach (var user in users)
            {
                AddSpecificNotifications(user, item, NotificationType.Web);
                if (user.EmailNotificationsEnabled && user.EmailConfirmed)
                {
                    AddSpecificNotifications(user, item, NotificationType.Email);
                }
                if (user.SmsNotificationsEnabled && user.PhoneNumberConfirmed)
                {
                    AddSpecificNotifications(user, item, NotificationType.Sms);
                }
            }
            return item;
        }

        private void AddSpecificNotifications(ApplicationUser user, Company company, NotificationType type)
        {
            if (company.CampaignNotifications == null)
                company.CampaignNotifications = new List<CampaignNotification>();
            if (company.Type != CompanyType.Send)
                company.CampaignNotifications.Add(new CampaignNotification()
                {
                    ApplicationUserId = user.Id,
                    BeenSent = false,
                    Event = CampaignNotificationEvent.CampaignStart,
                    Type = type
                });

            if (company.Type != CompanyType.Send)
                company.CampaignNotifications.Add(new CampaignNotification()
                {
                    ApplicationUserId = user.Id,
                    BeenSent = false,
                    Event = CampaignNotificationEvent.CampaignEnd,
                    Type = type
                });

            if (company.Type != CompanyType.Recieve)
                company.CampaignNotifications.Add(new CampaignNotification()
                {
                    ApplicationUserId = user.Id,
                    BeenSent = false,
                    Event = CampaignNotificationEvent.Sending,
                    Type = type
                });
        }
    }
}
