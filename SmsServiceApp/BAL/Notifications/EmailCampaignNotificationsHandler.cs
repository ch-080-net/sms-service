using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BAL.Notifications.Infrastructure;
using Model.DTOs;
using Model.Interfaces;
using WebApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BAL.Notifications
{
    public class EmailCampaignNotificationsHandler : NotificationHandler
    {
        public EmailCampaignNotificationsHandler(INotificationHandler notificationHandler
            , IUnitOfWork unitOfWork, IMapper mapper) : base(notificationHandler, unitOfWork, mapper)
        {
        }

        public override IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            var notifications = unitOfWork.EmailCampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Email
                && n.EmailCampaign.SendingTime <= DateTime.Now);
            var result = mapper.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<EmailNotificationDTO>>(notifications);
            result = result.Concat(base.notificationHandler.GetAllEmailNotifications());
            return result;
        }

        public override IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            var notifications = unitOfWork.EmailCampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Sms
                && n.EmailCampaign.SendingTime <= DateTime.Now);
            var result = mapper.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<SmsNotificationDTO>>(notifications);
            result = result.Concat(base.notificationHandler.GetAllSmsNotifications());
            return result;
        }

        public override int GetNumOfWebNotifications(string userId)
        {
            int result = unitOfWork.EmailCampaignNotifications
                .Get(n => n.EmailCampaign.SendingTime <= DateTime.Now && n.Type == NotificationType.Web
                && n.ApplicationUserId == userId && !n.BeenSent).Count();
            result += base.notificationHandler.GetNumOfWebNotifications(userId);
            return result;
        }

        public override IEnumerable<WebNotificationDTO> GetWebNotifications(string UserId, int quantity = 5)
        {
            var notifications = unitOfWork.EmailCampaignNotifications
                .Get(n => n.EmailCampaign.SendingTime <= DateTime.Now && n.Type == NotificationType.Web && n.ApplicationUserId == UserId)
                .OrderByDescending(x => x.EmailCampaign.SendingTime).Take(quantity);
            var result = mapper.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<WebNotificationDTO>>(notifications);
            result = result.Concat(base.notificationHandler.GetWebNotifications(UserId, quantity))
                .OrderByDescending(x => x.Time).Take(quantity);
            return result;
        }

        public override NotificationReportDTO GetWebNotificationsReport(string userId)
        {
            var result = new NotificationReportDTO();
            result.Notifications = GetWebNotifications(userId);
            var campaigns = unitOfWork.EmailCampaigns.Get(x => x.UserId == userId);

            result.MailingsPlannedToday = (from iter in campaigns
                                           where iter.SendingTime >= DateTime.Now
                                           && iter.SendingTime <= DateTime.Today.AddDays(1)
                                           select iter).Count();

            result += base.notificationHandler.GetWebNotificationsReport(userId);
            result.Notifications = result.Notifications.OrderByDescending(x => x.Time).Take(5);
            return result;
        }

        public override void SetAsSent(IEnumerable<NotificationDTO> notifications)
        {
            var actualNotifications = notifications.Where(x => x.Origin == NotificationOrigin.EmailCampaignReport);
            var campaignNotifications = unitOfWork.EmailCampaignNotifications.Get(x => actualNotifications.Any(y => y.Id == x.Id));
            foreach (var notification in campaignNotifications)
            {
                notification.BeenSent = true;
            }
            base.notificationHandler.SetAsSent(notifications);
        }

        public override void SetAsSent(string userId)
        {
            var campaignNotifications = unitOfWork.EmailCampaignNotifications.Get(n => n.ApplicationUserId == userId);
            foreach(var notification in campaignNotifications)
            {
                notification.BeenSent = true;
            }
            base.notificationHandler.SetAsSent(userId);
        }
    }
}
