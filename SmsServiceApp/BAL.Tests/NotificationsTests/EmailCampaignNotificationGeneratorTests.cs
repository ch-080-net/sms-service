using System;
using System.Collections.Generic;
using System.Text;
using BAL.Notifications;
using BAL.Notifications.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.DTOs;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BAL.Tests.NotificationsTests
{
    [TestFixture]
    public class EmailCampaignNotificationGeneratorTests
    {
        protected static Mock<IUnitOfWork> mockUnitOfWork;
        protected TestContext TestContext { get; set; }
        private INotificationsGenerator<EmailCampaign> generator;

        [SetUp]
        protected virtual void Initialize()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            generator = new EmailCampaignNotificationGenerator(mockUnitOfWork.Object);
            TestContext.WriteLine("Initialize test data");
        }

        [TearDown]
        protected virtual void Cleanup()
        {
            TestContext.WriteLine("Cleanup test data");
        }

        [Test]
        public void SupplyWithCampaignNotifications_NullCampaign_Null()
        {
            EmailCampaign item = null;

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void SupplyWithCampaignNotifications_UserDoesNotExist_CampaignWithoutNotifications()
        {
            EmailCampaign item = new EmailCampaign
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                UserId = "Hell"
            };
            var user = new ApplicationUser
            {
                Id = "Hell",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser>());

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result.EmailCampaignNotifications == null || !result.EmailCampaignNotifications.Any());
        }

        [Test]
        public void SupplyWithCampaignNotifications_UserInCampaign_CampaignWithNotifications()
        {
            var user = new ApplicationUser
            {
                Id = "Hell",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };
            EmailCampaign item = new EmailCampaign
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                UserId = "Hell",
                User = user
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser>());

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result.EmailCampaignNotifications.Any());
        }

        [Test]
        public void SupplyWithCampaignNotifications_UserInDb_CampaignWithNotifications()
        {
            var user = new ApplicationUser
            {
                Id = "Hell",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };
            EmailCampaign item = new EmailCampaign
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                UserId = "Hell",
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser>() { user });

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result.EmailCampaignNotifications.Any());
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void SupplyWithCampaignNotifications_Campaign_CampaignWithNotifications(bool a, bool b)
        {
            EmailCampaign item = new EmailCampaign
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                UserId = "Hell"
            };
            var user = new ApplicationUser
            {
                Id = "Hell",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = a,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = b
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser> { user });

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(!(result.EmailCampaignNotifications.Any(x => x.Type == NotificationType.Sms) ^ a)
                && !(result.EmailCampaignNotifications.Any(x => x.Type == NotificationType.Email) ^ b)
                && result.EmailCampaignNotifications.Any(x => x.Type == NotificationType.Web));
        }
    }
}
