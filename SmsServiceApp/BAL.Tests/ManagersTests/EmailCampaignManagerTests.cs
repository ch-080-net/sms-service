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
        private EmailCampaignManager manager;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            manager = new EmailCampaignManager(mockUnitOfWork.Object, mockMapper.Object, mockNotification.Object);
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void InsertWithRecepients_EmptyCampaign_FalseResult()
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
        public void InsertWithRecepients_CampignWithoutEmailInDatabase_TrueResult()
        {            
            EmailCampaignViewModel campaign = new EmailCampaignViewModel() { EmailAddress = "", Name = "test", Message = "test", };
            List<EmailRecipientViewModel> recepientsList = new List<EmailRecipientViewModel>();
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
            var result = manager.IncertWithRecepients(campaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void InsertWithRecepients_CampignWithEmailInDatabase_TrueResult()
        {
            EmailCampaignViewModel campaign = new EmailCampaignViewModel() { EmailAddress = "qq@qq.qq", Name = "test", Message = "test", };
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
            var result = manager.IncertWithRecepients(campaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void InsertWithRecepients_CampignWithWrongRecipientMapping_FalseResult()
        {
            EmailCampaignViewModel campaign = new EmailCampaignViewModel() { EmailAddress = "", Name = "test", Message = "test", };
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
            var result = manager.IncertWithRecepients(campaign, recepientsList);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_EmptyCampaign_FalseResult()
        {
            EmailCampaignViewModel campaign = new EmailCampaignViewModel();
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsAny<EmailCampaignViewModel>()))
                .Returns(new EmailCampaign() { Name = "test", Message = "test" });
            var result = manager.Update(campaign);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_CampaignWithEmailInDatabase_TrueResult()
        {
            EmailCampaignViewModel campaign = new EmailCampaignViewModel() { Id = 1, EmailAddress = "qq@qq.qq", Name = "test", Message = "test", };
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsAny<EmailCampaignViewModel>()))
                .Returns(new EmailCampaign() {Id = 1, Name = "test", Message = "test" });
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
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Update(It.IsAny<EmailCampaign>()));
            var result = manager.Update(campaign);
            Assert.IsTrue(result);
        }

        [Test]
        public void Update_CampaignWithoutEmailInDatabase_TrueResult()
        {
            EmailCampaignViewModel campaign = new EmailCampaignViewModel() { Id = 1, EmailAddress = "qq@qq.qq", Name = "test", Message = "test", };
            mockMapper.Setup(m => m.Map<EmailCampaign>(It.IsAny<EmailCampaignViewModel>()))
                .Returns(new EmailCampaign() { Id = 1, Name = "test", Message = "test" });
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), It.IsAny<Func<IQueryable<Email>,
                IOrderedQueryable<Email>>>(), It.IsAny<string>())).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Update(It.IsAny<EmailCampaign>()));
            var result = manager.Update(campaign);
            Assert.IsTrue(result);
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
        public void Delete_CampaignWithId_TrueResult()
        {
            mockUnitOfWork.Setup(u => u.EmailCampaigns.GetById(It.IsAny<int>())).Returns(new EmailCampaign() {Id = 1});
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Delete(It.IsAny<EmailCampaign>()));
            var result = manager.Delete(1);
            Assert.IsTrue(result);
        }

        [Test]
        public void Delete_CampaignWithWrongId_FalseResult()
        {
            mockUnitOfWork.Setup(u => u.EmailCampaigns.GetById(It.IsAny<int>())).Returns(value: null);
            mockUnitOfWork.Setup(u => u.EmailCampaigns.Delete(null)).Throws(new Exception("test"));
            var result = manager.Delete(1);
            Assert.IsFalse(result);
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