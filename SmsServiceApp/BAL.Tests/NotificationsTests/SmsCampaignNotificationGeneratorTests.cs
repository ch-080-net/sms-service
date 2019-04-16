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
    public class SmsCampaignNotificationGeneratorTests
    {
        protected static Mock<IUnitOfWork> mockUnitOfWork;
        protected TestContext TestContext { get; set; }
        private INotificationsGenerator<Company> generator;

        [SetUp]
        protected virtual void Initialize()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            generator = new SmsCampaignNotificationGenerator(mockUnitOfWork.Object);
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
            Company item = null;

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void SupplyWithCampaignNotifications_UsersInDb_CampaignWithNotifications()
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
            Company item = new Company
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                StartTime = DateTime.MinValue,
                EndTime = DateTime.MaxValue,
                ApplicationGroupId = 11,
                Type = CompanyType.SendAndRecieve
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser>() { user });

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result.CampaignNotifications.Any());
        }

        [Test]
        public void SupplyWithCampaignNotifications_NonExistentUsers_CampaignWithNotifications()
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
            Company item = new Company
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                StartTime = DateTime.MinValue,
                EndTime = DateTime.MaxValue,
                ApplicationGroupId = 11,
                Type = CompanyType.SendAndRecieve
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser>());

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(result.CampaignNotifications == null || !result.CampaignNotifications.Any());
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void SupplyWithCampaignNotifications_Campaign_CampaignWithNotifications(bool a, bool b)
        {
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
            Company item = new Company
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                StartTime = DateTime.MinValue,
                EndTime = DateTime.MaxValue,
                ApplicationGroupId = 11,
                Type = CompanyType.SendAndRecieve
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser> { user });

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That(!(result.CampaignNotifications.Any(x => x.Type == NotificationType.Sms) ^ a)
                && !(result.CampaignNotifications.Any(x => x.Type == NotificationType.Email) ^ b)
                && result.CampaignNotifications.Any(x => x.Type == NotificationType.Web));
        }

        [Test]
        [TestCase(CompanyType.Send)]
        [TestCase(CompanyType.SendAndRecieve)]
        [TestCase(CompanyType.Recieve)]
        public void SupplyWithCampaignNotifications_CampaignWithDifferentTypes_CampaignWithNotifications(CompanyType type)
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
            Company item = new Company
            {
                Id = 10,
                Name = "Hello, world",
                SendingTime = DateTime.MaxValue,
                StartTime = DateTime.MinValue,
                EndTime = DateTime.MaxValue,
                ApplicationGroupId = 11,
                Type = type
            };
            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(
                It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser> { user });

            var result = generator.SupplyWithCampaignNotifications(item);

            Assert.That((result.CampaignNotifications.Any(x => x.Event == CampaignNotificationEvent.Sending) ^ type == CompanyType.Recieve)
                && (result.CampaignNotifications.Any(x => x.Event == CampaignNotificationEvent.CampaignEnd) ^ type == CompanyType.Send)
                && (result.CampaignNotifications.Any(x => x.Event == CampaignNotificationEvent.CampaignStart) ^ type == CompanyType.Send));
        }
    }
}
