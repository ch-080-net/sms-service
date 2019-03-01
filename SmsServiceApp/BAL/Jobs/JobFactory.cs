using System;
using Quartz;
using Quartz.Spi;

namespace BAL.Jobs
{
    /// <summary>
    /// Implementation of IJobFactory which uses dependency injection to construct IJob instance.
    /// Jobs with unmanaged resources should implement IDisposable interface.
    /// </summary>
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceProvider serviceProvider;

        /// <param name="serviceProvider">Should contain entries to resolve instance of IJob</param>
        public JobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}
