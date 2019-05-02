using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.CodeViewModels;
using Moq;
using NUnit.Framework;
using WebApp.Models;
using BAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Services;
using Model.DTOs;
using Model.ViewModels.TestMessageViewModels;

namespace BAL.Tests.ManagersTests
{
    public class TestMessageManagerTests : TestInitializer
    {
        ITestMessageManager manager;

        [SetUp]
        protected override void Initialize()
        {
            IServiceCollection sc = new ServiceCollection();
            var mockSmsSender = new Mock<ISmsSender>();
            sc.AddSingleton<ISmsSender>(x => mockSmsSender.Object);

            manager = new TestMessageManager(mockUnitOfWork.Object, mockMapper.Object
                , sc.BuildServiceProvider().GetService<IServiceScopeFactory>());
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void SendTestMessage_ValidMessage_NoExceptionThrown()
        {
            var testMessage = new TestMessageViewModel();
            mockMapper.Setup(x => x.Map<MessageDTO>(It.Is<TestMessageViewModel>(y => y == testMessage)))
                .Returns(new MessageDTO());

            Assert.That(() => { manager.SendTestMessage(testMessage); }, Throws.Nothing);
        }

    }
}
