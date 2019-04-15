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
using WebApp.Services;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class EmailCampaignManagerTests : TestInitializer
    {
        private static Mock<INotificationsGenerator<EmailCampaign>> mockNotification = new Mock<INotificationsGenerator<EmailCampaign>>();
        private EmailCampaignManager manager = new EmailCampaignManager(mockUnitOfWork.Object, mockMapper.Object, mockNotification.Object);

        [Test]
        public void Insert_EmptyCampaign_FalseResult()
        {
            EmailCampaignViewModel emptyCampaign = new EmailCampaignViewModel();
            List<EmailRecipientViewModel> recepientsList = new List<EmailRecipientViewModel>();
            recepientsList.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "tt@tt.tt",
                BirthDate = DateTime.Now,
                Gender = "Male",
                KeyWords = "add",
                Name = "John",
                Surname = "Snow",
                Priority = "Low"
            });
            var result = manager.IncertWithRecepients(emptyCampaign, recepientsList);
            Assert.IsFalse(result);
        }

        [Test]
        public void Insert_CampignWithoutEmailInDatabase_TrueResult()
        {            
            EmailCampaignViewModel emptyCampaign = new EmailCampaignViewModel() { EmailAddress = "", Name = "test", Message = "test", };
            List<EmailRecipientViewModel> recepientsList = new List<EmailRecipientViewModel>();
            List<Email> Emails = new List<Email>();
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
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsAny<EmailCampaignViewModel>()))
                .Returns(new EmailCampaign() {Name = "test", Message = "test"});
            recepientsList.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "tt@tt.tt",
                BirthDate = DateTime.Now,
                Gender = "Male",
                KeyWords = "add",
                Name = "John",
                Surname = "Snow",
                Priority = "Low"
            });
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel, EmailRecipient>(It.IsAny<EmailRecipientViewModel>()))
                .Returns(new EmailRecipient() { BirthDate = DateTime.Now, Gender = 0, KeyWords = "add", Name = "John", Surname = "Snow", Priority = "Low"});
            var result = manager.IncertWithRecepients(emptyCampaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void Insert_CampignWithEmailInDatabase_TrueResult()
        {
            EmailCampaignViewModel emptyCampaign = new EmailCampaignViewModel() { EmailAddress = "", Name = "test", Message = "test", };
            List<EmailRecipientViewModel> recepientsList = new List<EmailRecipientViewModel>();
            List<Email> Emails = new List<Email>();
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
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(Emails);
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsAny<EmailCampaignViewModel>()))
                .Returns(new EmailCampaign() { Name = "test", Message = "test" });
            recepientsList.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "tt@tt.tt",
                BirthDate = DateTime.Now,
                Gender = "Male",
                KeyWords = "add",
                Name = "John",
                Surname = "Snow",
                Priority = "Low"
            });
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel, EmailRecipient>(It.IsAny<EmailRecipientViewModel>()))
                .Returns(new EmailRecipient() { BirthDate = DateTime.Now, Gender = 0, KeyWords = "add", Name = "John", Surname = "Snow", Priority = "Low" });
            var result = manager.IncertWithRecepients(emptyCampaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void Insert_CampignWithWrongRecipientMapping_FalseResult()
        {
            EmailCampaignViewModel emptyCampaign = new EmailCampaignViewModel() { EmailAddress = "", Name = "test", Message = "test", };
            List<EmailRecipientViewModel> recepientsList = new List<EmailRecipientViewModel>();
            List<Email> Emails = new List<Email>();
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
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Insert(It.IsAny<EmailCampaign>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsAny<EmailCampaignViewModel>()))
                .Returns(new EmailCampaign() { Name = "test", Message = "test" });
            recepientsList.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "tt@tt.tt",
                BirthDate = DateTime.Now,
                Gender = "Male",
                KeyWords = "add",
                Name = "John",
                Surname = "Snow",
                Priority = "Low"
            });
            var result = manager.IncertWithRecepients(emptyCampaign, recepientsList);
            Assert.IsFalse(result);
        }
    }
}