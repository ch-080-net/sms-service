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
            try
            {
                await SendMessages(result);
            }
            catch
            {
                return;
            }


        }

        private async Task SendMessages(IEnumerable<MessageDTO> messages)
        {
            SmsSender sms = new SmsSender();
            if (sms.Connect())
            {
                if (sms.OpenSession())
                {
                    sms.SendMessages(messages);
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
                    Console.WriteLine("Session error");
                }
            }
            else
            {
                Console.WriteLine("Connection error");
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
