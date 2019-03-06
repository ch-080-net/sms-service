using System;
using Quartz;
using Quartz.Impl;

namespace BAL.Jobs
{
    public static class MailingScheduler
    {
        /// <summary>
        /// Start scheduler for Mailing
        /// </summary>
        /// <param name="serviceProvider">Method calls custom implementation of IJobFactory which uses serviceProvider
        /// to create instance of IJob</param>
        public static async void Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = new JobFactory(serviceProvider);
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<Mailing>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MailingTrigger", "default")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
