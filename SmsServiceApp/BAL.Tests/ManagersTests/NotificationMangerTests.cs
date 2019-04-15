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

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class NotificationMangerTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private INotificationManager manager;

        [SetUp]
        public void SetUp()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();
            manager = new NotificationManager(mockUnitOfWork.Object, mockMapper.Object);
        }

        [Test]
        public void GetAllEmailNotifications_NotificationsInDb_EnumerationOfNotifications()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { new CampaignNotification() });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification() });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification() });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<EmailNotificationDTO>>
                (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<EmailNotificationDTO> { new EmailNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<EmailNotificationDTO> { new EmailNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<EmailNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<EmailNotificationDTO> { new EmailNotificationDTO() });

            var result = manager.GetAllEmailNotifications();

            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void GetAllEmailNotifications_NoNotificationInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<EmailNotificationDTO>>
                (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<EmailNotificationDTO>());

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<EmailNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<EmailNotificationDTO>());

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<EmailNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<EmailNotificationDTO>());

            var result = manager.GetAllEmailNotifications();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAllSmsNotifications_NotificationsInDb_EnumerationOfNotifications()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { new CampaignNotification() });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification() });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification() });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<SmsNotificationDTO>>
                (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<SmsNotificationDTO> { new SmsNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<SmsNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<SmsNotificationDTO> { new SmsNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<SmsNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<SmsNotificationDTO> { new SmsNotificationDTO() });

            var result = manager.GetAllSmsNotifications();

            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void GetAllSmsNotifications_NoNotificationInDb_EmptyEnumeration()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<SmsNotificationDTO>>
                (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<SmsNotificationDTO>());

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<SmsNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<SmsNotificationDTO>());

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<SmsNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<SmsNotificationDTO>());

            var result = manager.GetAllEmailNotifications();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void SetAsSent_DTOs_SuccesfullSave()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            manager.SetAsSent(new List<NotificationDTO> { new NotificationDTO() });

            mockUnitOfWork.Verify(m => m.Save(), Times.AtLeastOnce());
        }

        [Test]
        public void SetAsSent_DbExeption_ExceptionHandled()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockUnitOfWork.Setup(m => m.Save()).Throws(new DataException());

            manager.SetAsSent(new List<NotificationDTO> { new NotificationDTO() });

            Assert.That(() => { manager.SetAsSent(new List<NotificationDTO> { new NotificationDTO() }); }, Throws.Nothing);
        }

        [Test]
        public void SetAsSent_UserId_SuccesfullSave()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            manager.SetAsSent("GoodGirl");

            mockUnitOfWork.Verify(m => m.Save(), Times.AtLeastOnce());
        }

        [Test]
        public void SetAsSentForUserId_DbExeption_ExceptionHandled()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            mockUnitOfWork.Setup(m => m.Save()).Throws(new DataException());

            manager.SetAsSent("GoodGirl");

            Assert.That(() => { manager.SetAsSent(new List<NotificationDTO> { new NotificationDTO() }); }, Throws.Nothing);
        }

        [Test]
        [TestCase(5)]
        [TestCase(1)]
        [TestCase(-6)]
        public void GetWebNotificationsPage_NotificationsAvailable_CorrectQuantityOfDTOs(int a)
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { new CampaignNotification() });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification() });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification() });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>
                            (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            var result = manager.GetWebNotificationsPage("GoodGirl", a);

            Assert.That((a > 0) ? result.Count() <= a : result.Count() > 0);
        }

        [Test]
        [TestCase(5)]
        [TestCase(1)]
        [TestCase(-6)]
        public void GetWebNotificationsPage_NoNotificationsAvailable_EmptyEnumeration(int a)
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>
                            (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<WebNotificationDTO> { });

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<WebNotificationDTO> { });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<WebNotificationDTO> { });

            var result = manager.GetWebNotificationsPage("GoodGirl", a);

            Assert.IsFalse(result.Any());
        }

        [Test]
        public void GetWebNotificationsReport_NotificationsAvailable_ObjectWithNotifications()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { new CampaignNotification() });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification() });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification() });

            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser> { new ApplicationUser() });

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { new Company() });

            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaign>,
                IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaign> { new EmailCampaign() });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            var result = manager.GetWebNotificationsReport("GoodGirl");

            Assert.That(result.Notifications.Any());
        }

        [Test]
        public void GetWebNotificationsReport_IncorrectUserId_null()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { new CampaignNotification() });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification() });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification() });

            mockUnitOfWork.Setup(m => m.ApplicationUsers.Get(It.IsAny<Expression<Func<ApplicationUser, bool>>>()
                , It.IsAny<Func<IQueryable<ApplicationUser>,
                IOrderedQueryable<ApplicationUser>>>(), It.IsAny<string>()))
                .Returns(new List<ApplicationUser>());

            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>()
                , It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { new Company() });

            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaign>,
                IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaign> { new EmailCampaign() });

            mockMapper.Setup(m => m.Map<IEnumerable<CampaignNotification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<CampaignNotification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<Notification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<Notification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            mockMapper.Setup(m => m.Map<IEnumerable<EmailCampaignNotification>, IEnumerable<WebNotificationDTO>>
                (It.IsAny<IEnumerable<EmailCampaignNotification>>())).Returns(new List<WebNotificationDTO> { new WebNotificationDTO() });

            var result = manager.GetWebNotificationsReport("GoodGirl");

            Assert.That(result.Notifications, Is.Null);
        }

        [Test]
        public void AddNotificationsToUser_EmptyEnumeration_SuccessResult()
        {
            var result = manager.AddNotificationsToUser(new List<Notification>());

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void AddNotificationsToUser_NullEnumeration_ErrorResult()
        {
            List<Notification> item = null;
            var result = manager.AddNotificationsToUser(item);

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void AddNotificationsToUser_DbExeception_ErrorResult()
        {
            mockUnitOfWork.Setup(x => x.Save()).Throws(new DataException());
            List<Notification> item = new List<Notification> { new Notification() };

            var result = manager.AddNotificationsToUser(item);

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void AddNotificationsToUser_Notifications_SuccessResult()
        {
            mockUnitOfWork.Setup(n => n.Notifications.Insert(It.IsAny<Notification>()));
            List<Notification> item = new List<Notification> { new Notification() };

            var result = manager.AddNotificationsToUser(item);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetNumberOfWebNotifications_AvailableNotificationsInDb_NOfNotifications()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification> { new CampaignNotification() });

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification> { new Notification() });

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification> { new EmailCampaignNotification() });

            var result = manager.GetNumberOfWebNotifications("GoodGirl");

            Assert.That(result > 0);
        }

        [Test]
        public void GetNumberOfWebNotifications_NoNotificationsInDb_NOfNotifications()
        {
            mockUnitOfWork.Setup(m => m.CampaignNotifications.Get(It.IsAny<Expression<Func<CampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<CampaignNotification>,
                IOrderedQueryable<CampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<CampaignNotification>());

            mockUnitOfWork.Setup(m => m.Notifications.Get(It.IsAny<Expression<Func<Notification, bool>>>()
                , It.IsAny<Func<IQueryable<Notification>,
                IOrderedQueryable<Notification>>>(), It.IsAny<string>()))
                .Returns(new List<Notification>());

            mockUnitOfWork.Setup(m => m.EmailCampaignNotifications.Get(It.IsAny<Expression<Func<EmailCampaignNotification, bool>>>()
                , It.IsAny<Func<IQueryable<EmailCampaignNotification>,
                IOrderedQueryable<EmailCampaignNotification>>>(), It.IsAny<string>()))
                .Returns(new List<EmailCampaignNotification>());

            var result = manager.GetNumberOfWebNotifications("GoodGirl");

            Assert.That(result == 0);
        }



    }


}
