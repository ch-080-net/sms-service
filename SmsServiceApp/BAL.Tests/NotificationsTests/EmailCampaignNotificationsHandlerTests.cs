using System;
using System.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.DTOs;
using Moq;
using NUnit.Framework;
using WebApp.Models;
using System.Linq;
using BAL.Notifications;
using BAL.Notifications.Infrastructure;

namespace BAL.Tests.NotificationsTests
{
    public class EmailCampaignNotificationsHandlerTests : TestInitializer
    {
        Mock<INotificationHandler> mockBaseHandler;
        INotificationHandler handler;

        [SetUp]
        public void SetUp()
        {
            base.Initialize();
            this.mockBaseHandler = new Mock<INotificationHandler>();
            this.handler = new EmailCampaignNotificationsHandler(mockBaseHandler.Object, mockUnitOfWork.Object
                , mockMapper.Object);
        }

        [Test]
        public void GetAllEmailNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It
                .IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>
                { new EmailCampaignNotification {Type = NotificationType.Email
                , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} } });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<EmailNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
                .Returns(new List<EmailNotificationDTO> { new EmailNotificationDTO() });

            mockBaseHandler.Setup(m => m.GetAllEmailNotifications()).Returns(new List<EmailNotificationDTO>());

            var result = handler.GetAllEmailNotifications();

            Assert.That(result.Any());
        }

        [Test]
        public void GetAllEmailNotifications_NoNotificationsInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It
                .IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<EmailNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
                .Returns(new List<EmailNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetAllEmailNotifications()).Returns(new List<EmailNotificationDTO>());

            var result = handler.GetAllEmailNotifications();

            Assert.That(!result.Any());
        }

        [Test]
        public void GetAllSmsNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It
                .IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>
                { new EmailCampaignNotification {Type = NotificationType.Sms
                , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} } });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<SmsNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
                .Returns(new List<SmsNotificationDTO> { new SmsNotificationDTO() });

            mockBaseHandler.Setup(m => m.GetAllSmsNotifications()).Returns(new List<SmsNotificationDTO>());

            var result = handler.GetAllSmsNotifications();

            Assert.That(result.Any());
        }

        [Test]
        public void GetAllSmsNotifications_NoNotificationsInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It
                .IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<SmsNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
                .Returns(new List<SmsNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetAllSmsNotifications()).Returns(new List<SmsNotificationDTO>());

            var result = handler.GetAllSmsNotifications();

            Assert.That(!result.Any());
        }


    }
}
