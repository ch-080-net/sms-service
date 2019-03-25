using System;
using Quartz;
using Quartz.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace BAL.Jobs
{
    public static class NotificationScheduler
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<Notification>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("NotificationTrigger", "default")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
