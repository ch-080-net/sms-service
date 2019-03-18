using System;
using System.Threading.Tasks;
using Quartz;
using Model.Interfaces;
using AutoMapper;
using WebApp.Services;
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
			var service = serviceProvider.GetService<ISmsSender>();
			var result = serviceProvider.GetService<IMailingManager>().GetUnsentMessages();
            if (result.Any())
                await service.SendMessages(result);
        }
    }
}
