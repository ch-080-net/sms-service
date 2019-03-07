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
    public class Mailing : IJob, IDisposable
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
            if (!result.Any())
                return;
            try
            {
                await SendMessages(result);
            }
            catch
            {

            }
        }

        private async Task SendMessages(IEnumerable<MessageDTO> messages)
        {
			SmsSender sms = SmsSender.getInstance(serviceProvider.GetService<IServiceScopeFactory>());
            await sms.SendMessagesAsync(messages);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mailingManager.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
