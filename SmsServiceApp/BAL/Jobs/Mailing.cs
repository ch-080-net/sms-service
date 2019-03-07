using System;
using System.Threading.Tasks;
using Quartz;
using Model.Interfaces;
using AutoMapper;
using WebCustomerApp.Services;
using Model.DTOs;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace BAL.Jobs
{
    /// <summary>
    /// IJob implementation for sending messages through SMPP
    /// </summary>
    public class Mailing : IJob
    {
        private readonly IMapper mapper;
        private readonly IMailingManager mailingManager;
        private readonly IServiceProvider serviceProvider;

        public Mailing(IServiceProvider serviceProvider)
        {
            this.mapper = serviceProvider.GetService<IMapper>();
            this.mailingManager = serviceProvider.GetService<IMailingManager>();
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = await mailingManager.GetUnsentMessages();
            if (result.Any())
                await SendMessages(result);
        }

        private async Task SendMessages(IEnumerable<MessageDTO> messages)
        {
			SmsSender sms = await SmsSender.GetInstance(serviceProvider.GetService<IServiceScopeFactory>());
            try { await sms.SendMessagesAsync(messages); }
            finally { };
        }
    }
}
