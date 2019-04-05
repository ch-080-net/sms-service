using System;
using System.Threading.Tasks;
using Quartz;
using BAL.Managers;
using AutoMapper;
using WebApp.Services;
using Model.DTOs;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using BAL.Interfaces;

namespace BAL.Jobs
{
    /// <summary>
    /// IJob implementation for sending messages through SMPP
    /// </summary>
    public class Mailing : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public Mailing(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var emailsender = scope.ServiceProvider.GetService<IEmailSender>();
                var emails = scope.ServiceProvider.GetService<IEmailMailingManager>().GetUnsentEmails();
                if (emails.Any())
                {
                    emailsender.SendEmails(emails);
                }
                var service = scope.ServiceProvider.GetService<ISmsSender>();
                var result = scope.ServiceProvider.GetService<IMailingManager>().GetUnsentMessages();
                if (result.Any())
                    await service.SendMessages(result);
            }
        }
    }
}
