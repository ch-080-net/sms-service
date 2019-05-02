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
    public class SmsCampaignNotificationHandlerTests : TestInitializer
    {
        Mock<INotificationHandler> mockBaseHandler;
        INotificationHandler handler;

        [SetUp]
        public void SetUp()
        {
            this.mockBaseHandler = new Mock<INotificationHandler>();
            this.handler = new SmsCampaignNotificationHandler(mockBaseHandler.Object, mockUnitOfWork.Object
                , mockMapper.Object);
        }

        [Test]
        public void GetAllEmailNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It
                .IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>
                { new CampaignNotification {Type = NotificationType.Email, Event = CampaignNotificationEvent.Sending
                , Campaign = new Company { SendingTime = DateTime.Now} } });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<EmailNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
                .Returns(new List<EmailNotificationDTO> { new EmailNotificationDTO() });

            mockBaseHandler.Setup(m => m.GetAllEmailNotifications()).Returns(new List<EmailNotificationDTO>());

            var result = handler.GetAllEmailNotifications();

            Assert.That(result.Any());
        }

        [Test]
        public void GetAllEmailNotifications_NoNotificationsInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It
                .IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<EmailNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
                .Returns(new List<EmailNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetAllEmailNotifications()).Returns(new List<EmailNotificationDTO>());

            var result = handler.GetAllEmailNotifications();

            Assert.That(!result.Any());
        }

        [Test]
        public void GetAllSmsNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It
                .IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>
                { new CampaignNotification {Type = NotificationType.Sms, Event = CampaignNotificationEvent.Sending
                , Campaign = new Company { SendingTime = DateTime.Now} },
                new CampaignNotification {Type = NotificationType.Sms, Event = CampaignNotificationEvent.CampaignEnd
                , Campaign = new Company { EndTime = DateTime.Now} },
                new CampaignNotification {Type = NotificationType.Sms, Event = CampaignNotificationEvent.CampaignStart
                , Campaign = new Company { StartTime = DateTime.Now} }});

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<SmsNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
                .Returns(new List<SmsNotificationDTO> { new SmsNotificationDTO() });

            mockBaseHandler.Setup(m => m.GetAllSmsNotifications()).Returns(new List<SmsNotificationDTO>());

            var result = handler.GetAllSmsNotifications();

            Assert.That(result.Any());
        }

        [Test]
        public void GetAllSmsNotifications_NoNotificationsInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It
                .IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<SmsNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
                .Returns(new List<SmsNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetAllSmsNotifications()).Returns(new List<SmsNotificationDTO>());

            var result = handler.GetAllSmsNotifications();

            Assert.That(!result.Any());
        }

        [Test]
        public void GetNumOfWebNotifications_NotificationsInDb_Enumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It
                .IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>
                { new CampaignNotification {Type = NotificationType.Web, Event = CampaignNotificationEvent.Sending
                , Campaign = new Company { SendingTime = DateTime.Now} } });

            mockBaseHandler.Setup(m => m.GetNumOfWebNotifications(It.IsAny<string>())).Returns(0);

            var result = handler.GetNumOfWebNotifications("Herecore");

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        [TestCase(5)]
        [TestCase(3)]
        [TestCase(0)]
        [TestCase(-100)]
        public void GetWebNotifications_NotificationsInDb_Enumeration(int a)
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> {
                    new CampaignNotification {Type = NotificationType.Web, Event = CampaignNotificationEvent.CampaignEnd
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web, Event = CampaignNotificationEvent.CampaignStart
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web, Event = CampaignNotificationEvent.Sending
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                });
            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
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
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
                .Returns(new List<WebNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetWebNotifications(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<WebNotificationDTO>());

            var result = handler.GetWebNotifications("Hello");

            Assert.That(!result.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NotificationsInDb_FilledReport()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> {
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                });
            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
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

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company>());

            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser> { new ApplicationUser { ApplicationGroupId = 10} });

            var result = handler.GetWebNotificationsReport("Hello");

            Assert.That(result.Notifications.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NoNotificationsInDb_EmptyReport()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>
                , IEnumerable<WebNotificationDTO>>(It.IsAny<IEnumerable<CampaignNotification>>()))
                .Returns(new List<WebNotificationDTO>());

            mockBaseHandler.Setup(m => m.GetWebNotificationsReport(It.IsAny<string>()))
                .Returns(new NotificationReportDTO());

            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser> { new ApplicationUser { ApplicationGroupId = 10 } });

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company>());

            var result = handler.GetWebNotificationsReport("Hello");

            Assert.That(!result.Notifications.Any());
        }

        [Test]
        public void SetAsSent_NullEnumeration_ThrowsNothing()
        {
            var dbNotifications = new List<CampaignNotification> {
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { new Company() });

            List<NotificationDTO> notifications = null;

            Assert.That(() => { handler.SetAsSent(notifications); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_NoValidDTOs_ThowsNothing()
        {
            var dbNotifications = new List<CampaignNotification> {
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { new Company() });

            List<NotificationDTO> notifications = new List<NotificationDTO>();

            handler.SetAsSent(notifications);

            Assert.That(() => { handler.SetAsSent(notifications); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_ValidDTOs_NotificationsSetAsSent()
        {
            var dbNotifications = new List<CampaignNotification> {
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { new Company() });

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
            var dbNotifications = new List<CampaignNotification>();
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = null;

            Assert.That(() => { handler.SetAsSent(userId); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_EmptyUserId_ThrowsNothing()
        {
            var dbNotifications = new List<CampaignNotification>();
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = "";

            Assert.That(() => { handler.SetAsSent(userId); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_UserId_NotificationsSetAsSent()
        {
            var dbNotifications = new List<CampaignNotification> {
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                    new CampaignNotification {Type = NotificationType.Web
                        , Campaign = new Company { SendingTime = DateTime.Now} },
                };
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(dbNotifications);
            string userId = "Kappa";

            handler.SetAsSent(userId);

            Assert.That(dbNotifications.All(X => X.BeenSent == true));
        }
    }
}
