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
using Microsoft.AspNetCore.SignalR.Client;

namespace BAL.Jobs
{
    /// <summary>
    /// IJob implementation for sending messages through SMPP
    /// </summary>
    public class Mailing : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public Mailing(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var connection = new HubConnectionBuilder().WithUrl("http://localhost:53399/notificationHub").Build();
            await connection.StartAsync();
            await connection.InvokeAsync("SendNotification");
            await connection.StopAsync();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<ISmsSender>();
                var result = scope.ServiceProvider.GetService<IMailingManager>().GetUnsentMessages();
                if (result.Any())
                    await service.SendMessages(result);
            }
        }
    }
}
