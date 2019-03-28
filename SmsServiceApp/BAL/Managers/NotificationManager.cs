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

        public async Task<IEnumerable<SmsNotificationDTO>> GetAllSmsNotification()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.UtcNow
                                                                && n.Type == NotificationType.Sms);
            var senders = await userManager.GetUsersInRoleAsync("Admin");
            string senderPhone = senders.FirstOrDefault().PhoneNumber;

            var result = new List<SmsNotificationDTO>();
            foreach (var iter in notifications)
            {
                result.Add(mapper.Map<Notification, SmsNotificationDTO>(iter, opt =>
                    opt.AfterMap((src, dest) => dest.SenderPhone = senderPhone)));
            }
            return result;
        }

        public IEnumerable<WebNotificationDTO> GetAllWebNotification()
        {
            var notifications = unitOfWork.Notifications.Get(n => !n.BeenSent
                                                                && n.Time <= DateTime.UtcNow
                                                                && n.Type == NotificationType.Site);
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
    }
}
