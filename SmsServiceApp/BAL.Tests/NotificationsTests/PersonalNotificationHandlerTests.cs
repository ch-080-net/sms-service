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
    public class PersonalNotificationHandlerTests : TestInitializer
    {

        INotificationHandler handler;    

        [SetUp]
        public void SetUp()
        {
            base.Initialize();
            handler = new PersonalNotificationHandler(mockUnitOfWork.Object, mockMapper.Object);
        }

        [Test]
        public void GetAllEmailNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification { Time = DateTime.Now, Type = NotificationType.Email } });
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<EmailNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<EmailNotificationDTO> { new EmailNotificationDTO() });

            var result = handler.GetAllEmailNotifications();

            Assert.That(result.Any());
        }

        [Test]
        public void GetAllEmailNotifications_NoNotificationsInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> ());
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<EmailNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<EmailNotificationDTO>());

            var result = handler.GetAllEmailNotifications();

            Assert.That(!result.Any());
        }

        [Test]
        public void GetAllSmsNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification { Time = DateTime.Now, Type = NotificationType.Sms } });
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<SmsNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<SmsNotificationDTO> { new SmsNotificationDTO() });

            var result = handler.GetAllSmsNotifications();

            Assert.That(result.Any());
        }

        [Test]
        public void GetAllSmsNotifications_NoNotificationsInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<SmsNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<SmsNotificationDTO>());

            var result = handler.GetAllSmsNotifications();

            Assert.That(!result.Any());
        }

        [Test]
        public void GetNumOfWebNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification { Time = DateTime.Now, Type = NotificationType.Web } });

            var result = handler.GetNumOfWebNotifications("Herecore");

            Assert.That(result == 1);
        }

        [Test]
        [TestCase(5)]
        [TestCase(3)]
        [TestCase(0)]
        [TestCase(-100)]
        public void GetWebNotifications_NotificationsInDb_Enumeration(int a)
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> {
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Sms },
                });
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<WebNotificationDTO> {
                    new WebNotificationDTO(),
                    new WebNotificationDTO(),
                    new WebNotificationDTO(),
                    new WebNotificationDTO(),
                    new WebNotificationDTO(),
                    new WebNotificationDTO(),
                    new WebNotificationDTO(),
                    new WebNotificationDTO()
                });

            var result = handler.GetWebNotifications("Hello", a);

            Assert.That(result.Count() > 0);
        }
    }
}
