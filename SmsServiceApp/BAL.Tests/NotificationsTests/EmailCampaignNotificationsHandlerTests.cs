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
                , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} }, });

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

        [Test]
        public void GetNumOfWebNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification { Type = NotificationType.Web } });

            mockBaseHandler.Setup(m => m.GetNumOfWebNotifications(It.IsAny<string>())).Returns(0);

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
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> {
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                });
            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
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

            mockBaseHandler.Setup(m => m.GetWebNotifications(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<WebNotificationDTO>());

            var result = handler.GetWebNotifications("Hello", a);

            Assert.That(result.Count() > 0);
        }

        [Test]
        public void GetWebNotifications_NoNotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
                .Returns(new List<WebNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetWebNotifications(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<WebNotificationDTO>());

            var result = handler.GetWebNotifications("Hello");

            Assert.That(!result.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NotificationsInDb_FilledReport()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> {
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                });
            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
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

            mockBaseHandler.Setup(m => m.GetWebNotificationsReport(It.IsAny<string>()))
                .Returns(new NotificationReportDTO());

            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaign>,
                IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaign>());

            var result = handler.GetWebNotificationsReport("Hello");

            Assert.That(result.Notifications.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NoNotificationsInDb_EmptyReport()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());
            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<EmailCampaignNotification>>()))
                .Returns(new List<WebNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetWebNotificationsReport(It.IsAny<string>()))
                .Returns(new NotificationReportDTO());

            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaign>,
                IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaign>());

            var result = handler.GetWebNotificationsReport("Hello");

            Assert.That(!result.Notifications.Any());
        }

        [Test]
        public void SetAsSent_NullEnumeration_ThrowsNothing()
        {
            var dbNotifications = new List<EmailCampaignNotification> {
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            List<NotificationDTO> notifications = null;

            Assert.That(() => { handler.SetAsSent(notifications); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_NoValidDTOs_ThowsNothing()
        {
            var dbNotifications = new List<EmailCampaignNotification> {
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            List<NotificationDTO> notifications = new List<NotificationDTO>();

            handler.SetAsSent(notifications);

            Assert.That(() => { handler.SetAsSent(notifications); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_ValidDTOs_NotificationsSetAsSent()
        {
            var dbNotifications = new List<EmailCampaignNotification> {
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            List<NotificationDTO> notifications = new List<NotificationDTO> {
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.PersonalNotification },
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                    new NotificationDTO { Origin = NotificationOrigin.EmailCampaignReport },
                };

            handler.SetAsSent(notifications);

            Assert.That(dbNotifications.All(x => x.BeenSent == true));
        }

        [Test]
        public void SetAsSent_NullUserId_ThrowsNothing()
        {
            var dbNotifications = new List<EmailCampaignNotification>();
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = null;

            Assert.That(() => { handler.SetAsSent(userId); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_EmptyUserId_ThrowsNothing()
        {
            var dbNotifications = new List<EmailCampaignNotification>();
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = "";

            Assert.That(() => { handler.SetAsSent(userId); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_UserId_NotificationsSetAsSent()
        {
            var dbNotifications = new List<EmailCampaignNotification> {
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                    new EmailCampaignNotification {Type = NotificationType.Web
                        , EmailCampaign = new EmailCampaign { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = "Kappa";

            handler.SetAsSent(userId);

            Assert.That(dbNotifications.All(X => X.BeenSent == true));
        }

    }
}
