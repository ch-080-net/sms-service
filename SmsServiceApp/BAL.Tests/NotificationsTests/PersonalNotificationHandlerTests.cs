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
                .Returns(new List<Notification>());
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
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Web },
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

        [Test]
        public void GetWebNotifications_NoNotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<WebNotificationDTO>());

            var result = handler.GetWebNotifications("Hello");

            Assert.That(!result.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NotificationsInDb_FilledReport()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> {
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Web },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Web },
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

            var result = handler.GetWebNotificationsReport("Hello");

            Assert.That(result.Notifications.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NoNotificationsInDb_EmptyReport()
        {
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());
            mockMapper.Setup(m => m.Map<IEnumerable<Notification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<Notification>>()))
                .Returns(new List<WebNotificationDTO>());

            var result = handler.GetWebNotificationsReport("Hello");

            Assert.That(!result.Notifications.Any());
        }

        [Test]
        public void SetAsSent_NullEnumeration_ThrowsNothing()
        {
            var dbNotifications = new List<Notification> {
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Sms },
                };
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            List<NotificationDTO> notifications = null;

            Assert.That(() => { handler.SetAsSent(notifications); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_NoValidDTOs_ThowsNothing()
        {
            var dbNotifications = new List<Notification> {
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Sms },
                };
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            List<NotificationDTO> notifications = new List<NotificationDTO>();

            handler.SetAsSent(notifications);

            Assert.That(() => { handler.SetAsSent(notifications); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_ValidDTOs_NotificationsSetAsSent()
        {
            var dbNotifications = new List<Notification> {
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Sms },
                };
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            List<NotificationDTO> notifications = new List<NotificationDTO> {
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.CampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                };

            handler.SetAsSent(notifications);

            Assert.That(dbNotifications.All(x => x.BeenSent == true));
        }

        [Test]
        public void SetAsSent_NullUserId_ThrowsNothing()
        {
            var dbNotifications = new List<Notification>();
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = null;

            Assert.That(() => { handler.SetAsSent(userId); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_EmptyUserId_ThrowsNothing()
        {
            var dbNotifications = new List<Notification>();
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = "";

            Assert.That(() => { handler.SetAsSent(userId); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_UserId_NotificationsSetAsSent()
        {
            var dbNotifications = new List<Notification> {
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MinValue, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.Now, Type = NotificationType.Sms },
                    new Notification { Time = DateTime.MaxValue, Type = NotificationType.Sms },
                };
            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = "Kappa";

            handler.SetAsSent(userId);

            Assert.That(dbNotifications.All(X => X.BeenSent == true));
        }
    }
}
