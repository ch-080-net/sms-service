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
    public class Notification : IJob
    {

        private readonly IServiceScopeFactory serviceScopeFactory;

        public Notification(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var connection = new HubConnectionBuilder().WithUrl("http://localhost:51238/notificationHub").Build();
            await connection.StartAsync();
            await connection.InvokeAsync("SendNotification");
            await connection.StopAsync();
        }
    }
}
