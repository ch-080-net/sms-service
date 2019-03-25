using System;
using Quartz;
using Quartz.Spi;
using Microsoft.Extensions.DependencyInjection;

namespace BAL.Jobs
{
    /// <summary>
    /// Implementation of IJobFactory which uses dependency injection to construct IJob instance.
    /// Jobs with unmanaged resources should implement IDisposable interface.
    /// </summary>
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScope serviceScope;

        /// <param name="serviceProvider">Should contain entries to resolve instance of IJob</param>
        public JobFactory(IServiceProvider serviceProvider)
        {
            this.serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {                        
            var job = serviceScope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return job;
        }

        public void ReturnJob(IJob job)
        {
            serviceScope.Dispose();
        }
    }
}
