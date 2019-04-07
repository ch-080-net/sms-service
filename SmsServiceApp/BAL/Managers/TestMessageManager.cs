using AutoMapper;
using BAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Model.DTOs;
using Model.Interfaces;
using Model.ViewModels.TestMessageViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Services;

namespace BAL.Managers
{
    public class TestMessageManager : BaseManager, ITestMessageManager
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public TestMessageManager(IUnitOfWork unitOfWork, IMapper mapper, IServiceScopeFactory serviceScopeFactory) : base(unitOfWork, mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void SendTestMessage(TestMessageViewModel message)
        {
            MessageDTO messageDTO =  mapper.Map<MessageDTO>(message);
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<ISmsSender>();
                service.SendMessage(messageDTO);
            }
        }
    }
}
