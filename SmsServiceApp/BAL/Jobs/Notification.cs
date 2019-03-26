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
using Microsoft.AspNetCore.SignalR;
using BAL.Hubs;

namespace BAL.Jobs
{
    public class Notification : IJob
    {

        private readonly IServiceScopeFactory serviceScopeFactory;

        public Notification(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var hub = scope.ServiceProvider.GetService<IHubContext<NotificationHub>>();
                await hub.Clients.All.SendAsync("GetNotification", "Hi!");
            }
        }
    }
}
