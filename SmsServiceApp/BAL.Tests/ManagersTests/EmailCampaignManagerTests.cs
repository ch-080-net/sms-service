using System;
using System.Collections.Generic;
using BAL.Managers;
using BAL.Notifications.Infrastructure;
using Model.ViewModels.EmailCampaignViewModels;
using Model.ViewModels.EmailRecipientViewModels;
using NUnit.Framework;
using Moq;
using WebApp.Models;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.Configuration;
using WebApp.Services;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class EmailCampaignManagerTests : TestInitializer
    {
        private static Mock<INotificationsGenerator<EmailCampaign>> mockNotification = new Mock<INotificationsGenerator<EmailCampaign>>();
        private EmailCampaignManager manager;
        private List<Email> Emails;
        private List<EmailRecipientViewModel> recipientViewModels;
        private EmailCampaignViewModel emailCampaignViewModel;
        private EmailRecipient emailRecipient;
        private EmailCampaign emailCampaign;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            manager = new EmailCampaignManager(mockUnitOfWork.Object, mockMapper.Object, mockNotification.Object);
            Emails = new List<Email>();
            Emails.Add(
                new Email()
                {
                    Id = 10,
                    EmailAddress = "qq@qq.qq"
                });
            Emails.Add(
                new Email()
                {
                    Id = 11,
                    EmailAddress = "tt@tt.tt"
                });
            recipientViewModels = new List<EmailRecipientViewModel>();
            recipientViewModels.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "tt@tt.tt",
                BirthDate = DateTime.Now,
                Gender = "Male",
                KeyWords = "add",
                Name = "John",
                Surname = "Snow",
                Priority = "Low",
                Id = 0,
                CompanyId = 0
            });
            emailCampaignViewModel = new EmailCampaignViewModel()
            {
                Id = 0,
                EmailAddress = "qq@qq.qq",
                Description = "test",
                Message = "test",
                Name = "test",
                SendingTime = DateTime.Now,
                UserId = "1cf1c047-9687-4663-be13-09e4a99f35f3",
                RecipientsCount = 1,
                EmailId = 0
            };
            emailRecipient = new EmailRecipient()
            {
                BirthDate = DateTime.Now,
                Gender = 0,
                IsSend = 0,
                KeyWords = "test",
                Name = "test",
                Priority = "Low",
                Surname = "test"
            };
            emailCampaign = new EmailCampaign()
            {
                Name = "test",
                Description = "test",
                Message = "test",
                SendingTime = DateTime.Now
            };
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void InsertWithRecepients_EmptyCampaign_ThrowNullReferenceException()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(value: null);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(Emails);
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockNotification.Setup(n => n.SupplyWithCampaignNotifications(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel, EmailRecipient>(recipientViewModels[0]))
                .Returns(emailRecipient);
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsNotNull<EmailRecipient>()));
            EmailCampaignViewModel emptyCampaign = new EmailCampaignViewModel();
            List<EmailRecipientViewModel> recepientsList = new List<EmailRecipientViewModel>();
            Assert.Throws<NullReferenceException>(() => manager.IncertWithRecepients(emptyCampaign, recipientViewModels));
        }

        [Test]
        public void InsertWithRecepients_CampignWithoutEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockNotification.Setup(n => n.SupplyWithCampaignNotifications(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel, EmailRecipient>(recipientViewModels[0]))
                .Returns(emailRecipient);
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsNotNull<EmailRecipient>()));
            Assert.DoesNotThrow(() => manager.IncertWithRecepients(emailCampaignViewModel, recipientViewModels));
        }

        [Test]
        public void InsertWithRecepients_CampignWithEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(Emails);
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockNotification.Setup(n => n.SupplyWithCampaignNotifications(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel, EmailRecipient>(recipientViewModels[0]))
                .Returns(emailRecipient);
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsNotNull<EmailRecipient>()));
            Assert.DoesNotThrow(() => manager.IncertWithRecepients(emailCampaignViewModel, recipientViewModels));
        }

        [Test]
        public void InsertWithRecepients_CampignWithWrongRecipientMapping_ThrowNullReferenceException()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(Emails);
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockNotification.Setup(n => n.SupplyWithCampaignNotifications(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel, EmailRecipient>(It.IsNotNull<EmailRecipientViewModel>()))
                .Throws<NullReferenceException>();
            recipientViewModels[0] = null;
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsNotNull<EmailRecipient>()));
            Assert.Throws<NullReferenceException>(() => manager.IncertWithRecepients(emailCampaignViewModel, recipientViewModels));
        }

        [Test]
        public void Update_EmptyCampaign_ThrowNullReferenceException()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(Emails);
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Update(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<NullReferenceException>(() => manager.Update(null));
        }

        [Test]
        public void Update_CampaignWithEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(Emails);
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Update(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(emailCampaignViewModel));
        }

        [Test]
        public void Update_CampaignWithoutEmailInDatabase_TrueResult()
        {
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsNotNull<EmailCampaignViewModel>()))
                .Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsNotNull<Email>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Update(It.IsNotNull<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(emailCampaignViewModel));
        }

        [Test]
        public void GetCampaigns_WrongUser_NullResult()
        {
            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>(), It
                .IsAny<Func<IQueryable<EmailCampaign>,
                    IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>())).Returns(new List<EmailCampaign>());
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<EmailCampaign>, List<EmailCampaignViewModel>>(
                        It.IsAny<IEnumerable<EmailCampaign>>()))
                .Returns(value: null);
            var result = manager.GetCampaigns("", 1, 1, "");
            Assert.IsNull(result);
        }

        [Test]
        public void GetCampaigns_RightUser_ListOfCamapaignsResult()
        {
            List<EmailCampaign> campaigns = new List<EmailCampaign>();
            campaigns.Add(
                new EmailCampaign()
                {
                    Name = "test",
                    Description = "test",
                    EmailId = 1,
                    Id = 1,
                }
                );
            List<EmailCampaignViewModel> campaignsViewModels = new List<EmailCampaignViewModel>();
            campaignsViewModels.Add(
                new EmailCampaignViewModel()
                {
                    Name = "test",
                    Description = "test",
                    EmailAddress = "",
                    Id = 1,
                }
            );
            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>(), It
                .IsAny<Func<IQueryable<EmailCampaign>,
                    IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>())).Returns(campaigns);
            mockUnitOfWork.Setup(m => m.Emails.GetById(It.IsAny<int>())).Returns(new Email());
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<EmailCampaign>, List<EmailCampaignViewModel>>(
                        It.IsAny<IEnumerable<EmailCampaign>>()))
                .Returns(campaignsViewModels);
            var result = manager.GetCampaigns("1cf1c047-9687-4663-be13-09e4a99f35f3", 1, 1, "");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Delete_CampaignWithId_DoesNotThrow()
        {
            mockUnitOfWork.Setup(u => u.EmailCampaigns.GetById(It.IsAny<int>())).Returns(emailCampaign);
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Delete(It.IsNotNull<EmailCampaign>()));
            Assert.DoesNotThrow(() => manager.Delete(1));
        }

        [Test]
        public void Delete_CampaignWithWrongId_ThrowNullReferenceException()
        {
            mockUnitOfWork.Setup(u => u.EmailCampaigns.GetById(It.IsAny<int>())).Returns((EmailCampaign)null);
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Delete(It.IsAny<EmailCampaign>())).Throws(new NullReferenceException());
            Assert.Throws<NullReferenceException>(() => manager.Delete(1));
        }

        [Test]
        public void GetById_GetCampaign_EmailCampaignObjectResult()
        {
            mockUnitOfWork.Setup(u => u.EmailCampaigns.GetById(It.IsAny<int>())).Returns(new EmailCampaign() { Id = 1 , EmailId = 1});
            mockUnitOfWork.Setup(u => u.Emails.GetById(It.IsAny<int>())).Returns(new Email() { Id = 1, EmailAddress = "qq@qq.qq"});
            mockMapper.Setup(m => m.Map<EmailCampaignViewModel>(It.IsAny<EmailCampaign>()))
                .Returns(new EmailCampaignViewModel() {Id = 1, EmailAddress = "qq@qq.qq"});
            var result = manager.GetById(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetCampaignsCount_GetCampaignCount_IntResult()
        {
            List<EmailCampaign> campaigns = new List<EmailCampaign>();
            campaigns.Add(
                new EmailCampaign()
                {
                    Name = "test",
                    Description = "test",
                    EmailId = 1,
                    Id = 1,
                }
            );
            mockUnitOfWork.Setup(m => m.EmailCampaigns.Get(It.IsAny<Expression<Func<EmailCampaign, bool>>>(), It
                .IsAny<Func<IQueryable<EmailCampaign>,
                    IOrderedQueryable<EmailCampaign>>>(), It.IsAny<string>())).Returns(campaigns);
            var result = manager.GetCampaignsCount("1cf1c047-9687-4663-be13-09e4a99f35f3", "");
            Assert.AreEqual(1, result);
        }
    }
}