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
    public class PersonalNotificationBuilderTests
    {
        [Test]
        public void Build_NullUserInConstructor_ExceptionThrown()
        {
            ApplicationUser user = null;

            Assert.Throws<NullReferenceException>(() => { new PersonalNotificationBuilder(user).Build(); });
        }

        [Test]
        public void Build_UserHasNoSmsNotificationsAllowed_CollectionWithoutSmsNotification()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = false,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };

            var result = new PersonalNotificationBuilder(user).Build();

            Assert.That(!result.Any(x => x.Type == NotificationType.Sms));
        }

        [Test]
        public void Build_UserHasNoEmailNotificationsAllowed_CollectionWithoutEmailNotification()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = false
            };

            var result = new PersonalNotificationBuilder(user).Build();

            Assert.That(!result.Any(x => x.Type == NotificationType.Email));
        }

        [Test]
        public void Build_UserHasEmailAndSmsNotificationsAllowed_CollectionWithoutEmailAndSmsNotification()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };

            var result = new PersonalNotificationBuilder(user).Build();

            Assert.That(result.Any(x => x.Type == NotificationType.Email) && result.Any(x => x.Type == NotificationType.Email));
        }

        [Test]
        public void Build_TitleAndMessageSpecified_NotificationsContainsTitlesAndMessages()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };
            string title = "Emperor Protects!";
            string message = "For the Emperor!";

            var result = new PersonalNotificationBuilder(user)
                .SetMessage(title, message).Build();

            Assert.That(result.All(x => x.Message == message && x.Title == title));
        }

        [Test]
        public void Build_TimeSpecified_NotificationsContainsTime()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };

            var result = new PersonalNotificationBuilder(user)
                .SetTime(DateTime.MaxValue).Build();

            Assert.That(result.All(x => x.Time == DateTime.MaxValue));
        }

        [Test]
        public void Build_HrefSet_NotificationsContainsHrefs()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };
            string href = "https://4chan.org/";

            var result = new PersonalNotificationBuilder(user)
                .SetHref(href).Build();

            Assert.That(result.All(x => x.Href == href));
        }

        [Test]
        public void Build_HrefGenerated_NotificationsContainsHrefs()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "GreatHuman",
                PhoneNumber = "+380566754454",
                PhoneNumberConfirmed = true,
                SmsNotificationsEnabled = true,
                Email = "aaa@bbb.com",
                EmailConfirmed = true,
                EmailNotificationsEnabled = true
            };
            var mockUrlHelper = new Mock<IUrlHelper>();
            string href = "https://4chan.org/";
            string action = "Hello";
            string controller = "Heaven";
            object values = new { Devil = "!God" };
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(href);

            var result = new PersonalNotificationBuilder(user)
                .GenerateHref(mockUrlHelper.Object, action, controller, values).Build();

            Assert.That(result.All(x => x.Href == href));
        }
    }

}
