using System;
using System.Threading.Tasks;
using Quartz;
using Model.Interfaces;
using AutoMapper;

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
            Console.WriteLine(result);
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
