using System;
using System.Threading.Tasks;
using Quartz;
using Model.Interfaces;
using AutoMapper;
using WebCustomerApp.Services;
using Model.DTOs;
using System.Linq;
using System.Collections.Generic;

namespace BAL.Jobs
{
    /// <summary>
    /// IJob implementation for sending messages through SMPP
    /// </summary>
    public class Mailing : IJob, IDisposable
    {
        private readonly IMailingManager mailingManager;
        private readonly IMapper mapper;

        public Mailing(IMailingManager mailingManager, IMapper mapper)
        {
            this.mailingManager = mailingManager;
            this.mapper = mapper;
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
                return;
            }
            await mailingManager.MarkAsSent(result);
        }

        private async Task SendMessages(IEnumerable<MessageDTO> messages)
        {
			SmsSender sms = new SmsSender();
            if (sms.Connect())
            {
                if (sms.OpenSession())
                {
                    await sms.SendMessagesAsync(messages);
					if (sms.CloseSession())
					{
						sms.Disconnect();
						Console.WriteLine("Connection close");
					}
					else
						Console.WriteLine("Could not close session");
				}
				else
                {
					throw new Exception("Session error");
                }
            }
            else
            {
				throw new Exception("Connection error");
            }
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
