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
        protected readonly IServiceScopeFactory serviceScopeFactory;

        /// <param name="serviceProvider">Should contain entries to resolve instance of IJob</param>
        public JobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }
            
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
