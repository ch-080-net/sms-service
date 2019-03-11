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
        private readonly IServiceProvider serviceProvider;

        public Mailing(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = serviceProvider.GetService<IMailingManager>().GetUnsentMessages();
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
