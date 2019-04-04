using System;
using System.Threading.Tasks;
using Quartz;
using Model.Interfaces;
using AutoMapper;
using WebApp.Services;
using Model.DTOs;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using BAL.Hubs;
using BAL.Managers;

namespace BAL.Jobs
{
    public class Notification : IJob
    {

        private readonly IServiceScopeFactory serviceScopeFactory;

        public Notification(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await SendEmails();            
            await SendWebNotification();
            await SendSms();
        }

        private async Task SendEmails()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var manager = scope.ServiceProvider.GetService<INotificationManager>();
                var sender = scope.ServiceProvider.GetService<IEmailSender>();

                var emailNotifications = manager.GetAllEmailNotifications();

                foreach (var iter in emailNotifications)
                {
                    await sender.SendEmailAsync(iter.Email, iter.Title, iter.Message);
                }

                manager.SetAsSent(emailNotifications);
            }
        }

        private async Task SendSms()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var manager = scope.ServiceProvider.GetService<INotificationManager>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();

                var smsNotifications = manager.GetAllSmsNotifications();

                var smsMessages = mapper.Map<IEnumerable<SmsNotificationDTO>, IEnumerable<MessageDTO>>(smsNotifications);

                await scope.ServiceProvider.GetService<ISmsSender>().SendMessages(smsMessages);                
                manager.SetAsSent(smsNotifications);
            }
        }

        private async Task SendWebNotification()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetService<IHubContext<NotificationHub>>();
                var manager = scope.ServiceProvider.GetService<INotificationManager>();
                var webNotifications = manager.GetAllWebNotifications();
                foreach (var iter in webNotifications)
                { 
                    await hubContext.Clients.User(iter.UserId).SendAsync("GetNotification", iter);
                }
            }
        }
    }
}
