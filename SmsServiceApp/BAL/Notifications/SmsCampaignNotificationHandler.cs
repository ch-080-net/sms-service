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
    public class SmsCampaignNotificationHandler : NotificationHandler
    {
        public SmsCampaignNotificationHandler(INotificationHandler notificationHandler, IUnitOfWork unitOfWork
            , IMapper mapper) : base(notificationHandler, unitOfWork, mapper)
        {
        }

        public override IEnumerable<EmailNotificationDTO> GetAllEmailNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Email
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<EmailNotificationDTO>>(notifications);
            result = result.Concat(base.notificationHandler.GetAllEmailNotifications());
            return result;
        }

        public override IEnumerable<SmsNotificationDTO> GetAllSmsNotifications()
        {
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                !n.BeenSent
                && n.Type == NotificationType.Sms
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now));
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<SmsNotificationDTO>>(notifications);
            result = result.Concat(base.notificationHandler.GetAllSmsNotifications());
            return result;
        }

        public override IEnumerable<WebNotificationDTO> GetWebNotifications(string userId, int quantity = 5)
        {
            quantity = (quantity < 1) ? 5 : quantity;
            var result = GetWebNotificationsForSmsCampaign(userId, quantity);
            result = result.Concat(base.notificationHandler.GetWebNotifications(userId, quantity))
                .OrderByDescending(x => x.Time).Take(quantity);
            return result;
        }

        private IEnumerable<WebNotificationDTO> GetWebNotificationsForSmsCampaign(string userId, int quantity = 5)
        {
            quantity = (quantity < 1) ? 5 : quantity;
            var notifications = unitOfWork.CampaignNotifications.Get(n =>
                n.Type == NotificationType.Web
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now)
                && n.ApplicationUserId == userId)
                .OrderByDescending(x => Comparer(x)).Take(quantity);
            var result = mapper.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>(notifications);
            return result;
        }

        private DateTime Comparer(CampaignNotification notification)
        {
            switch (notification.Event)
            {
                case CampaignNotificationEvent.CampaignStart:
                    return notification.Campaign.StartTime;
                case CampaignNotificationEvent.CampaignEnd:
                    return notification.Campaign.EndTime;
                case CampaignNotificationEvent.Sending:
                    return notification.Campaign.SendingTime;
                default:
                    return DateTime.MinValue;
            }
        }

        public override NotificationReportDTO GetWebNotificationsReport(string userId)
        {
            var result = new NotificationReportDTO();
            var user = unitOfWork.ApplicationUsers.Get(x => x.Id == userId).FirstOrDefault();
            if (user == null)
                return result;
            result.Notifications = GetWebNotificationsForSmsCampaign(userId);
            var campaigns = unitOfWork.Companies.Get(x => x.ApplicationGroupId == user.ApplicationGroupId);

            result.VotingsInProgress = (from iter in campaigns
                                        where iter.StartTime <= DateTime.Now
                                        && iter.EndTime >= DateTime.Now
                                        select iter).Count();

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
            if (notifications == null || !notifications.Any())
                return;
            var actualNotifications = notifications.Where(x => x.Origin == NotificationOrigin.CampaignReport);
            var campaignNotifications = unitOfWork.CampaignNotifications.Get(x => actualNotifications.Any(y => y.Id == x.Id));
            foreach (var notification in campaignNotifications)
            {
                notification.BeenSent = true;
            }
            base.notificationHandler.SetAsSent(notifications);
        }

        public override void SetAsSent(string userId)
        {
            var emailCampaignNotifications = unitOfWork.CampaignNotifications.Get(n => n.ApplicationUserId == userId);
            foreach (var notification in emailCampaignNotifications)
            {
                notification.BeenSent = true;
            }
            base.notificationHandler.SetAsSent(userId);
        }

        public override int GetNumOfWebNotifications(string userId)
        {
            int result = unitOfWork.CampaignNotifications.Get(n =>
                n.Type == NotificationType.Web
                && (n.Event == CampaignNotificationEvent.CampaignStart && n.Campaign.StartTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.CampaignEnd && n.Campaign.EndTime <= DateTime.Now
                || n.Event == CampaignNotificationEvent.Sending && n.Campaign.SendingTime <= DateTime.Now)
                && n.ApplicationUserId == userId && !n.BeenSent).Count();
            result += base.notificationHandler.GetNumOfWebNotifications(userId);
            return result;
        }
    }
}
