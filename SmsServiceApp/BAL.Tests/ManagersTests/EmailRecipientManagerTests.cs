using System;
using BAL.Managers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.Components.DictionaryAdapter;
using Model.ViewModels.EmailRecipientViewModels;
using WebApp.Models;
using Moq;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class EmailRecipientManagerTests: TestInitializer
    {
        private EmailRecipientManager manager;

        [SetUp]
        protected override void Initialize()
        {
            manager = new EmailRecipientManager(mockUnitOfWork.Object, mockMapper.Object);
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void Insert_EmailRecipientWithEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailRecipient>(It.IsAny<EmailRecipientViewModel>())).Returns(new EmailRecipient() {EmailId = 10});

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
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(new EmailRecipientViewModel() {EmailAddress = "qq@qq.qq"}, 1));
        }

        [Test]
        public void Insert_EmailRecipientWithoutEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailRecipient>(It.IsAny<EmailRecipientViewModel>())).Returns(new EmailRecipient());
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), null, null)).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(new EmailRecipientViewModel() { EmailAddress = "qq@qq.qq" }, 1));
        }

        [Test]
        public void Insert_WrongEmailRecipient_ThrowException()
        {
            mockMapper.Setup(m => m.Map<EmailRecipient>(It.IsAny<EmailRecipientViewModel>())).Returns(new EmailRecipient());
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), null, null)).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Insert(It.IsAny<EmailRecipient>())).Throws(new Exception());
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<Exception>(() => manager.Insert(new EmailRecipientViewModel() { EmailAddress = "qq@qq.qq" }, 1));
        }

        [Test]
        public void Update_EmailRecipientWithEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailRecipient>(It.IsAny<EmailRecipientViewModel>())).Returns(new EmailRecipient() { EmailId = 10 });

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
            mockUnitOfWork.Setup(u => u.EmailRecipients.Update(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(new EmailRecipientViewModel() { EmailAddress = "qq@qq.qq" }));
        }

        [Test]
        public void Update_EmailRecipientWithoutEmailInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<EmailRecipient>(It.IsAny<EmailRecipientViewModel>())).Returns(new EmailRecipient());
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), null, null)).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Update(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(new EmailRecipientViewModel() { EmailAddress = "qq@qq.qq" }));
        }

        [Test]
        public void Update_WrongEmailRecipient_ThrowException()
        {
            mockMapper.Setup(m => m.Map<EmailRecipient>(It.IsAny<EmailRecipientViewModel>())).Returns(new EmailRecipient());
            mockUnitOfWork.Setup(u => u.Emails.Get(It.IsAny<Expression<Func<Email, bool>>>(), null, null)).Returns(new List<Email>());
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.EmailRecipients.Update(It.IsAny<EmailRecipient>())).Throws(new Exception());
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<Exception>(() => manager.Update(new EmailRecipientViewModel() { EmailAddress = "qq@qq.qq" }));
        }

        [Test]
        public void GetEmailRecipientsById_GetEmailRecipient_ReturnEmailRecipient()
        {
            mockUnitOfWork.Setup(u => u.EmailRecipients.GetById(It.IsAny<int>()))
                .Returns(new EmailRecipient() {EmailId = 10});
            mockUnitOfWork.Setup(u => u.Emails.GetById(It.IsAny<int>()))
                .Returns(new Email() {EmailAddress = "qq@qq.qq"});
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel>(It.IsAny<EmailRecipient>()))
                .Returns(new EmailRecipientViewModel() {EmailAddress = "qq@qq.qq"});
            Assert.That(manager.GetEmailRecipientById(10), Is.TypeOf<EmailRecipientViewModel>());
        }

        [Test]
        public void GetEmailRecipientsById_WithoutEmailRecipientInDatabase_ReturnEmailRecipient()
        {
            mockUnitOfWork.Setup(u => u.EmailRecipients.GetById(It.IsAny<int>()))
                .Returns(value: null);
            mockUnitOfWork.Setup(u => u.Emails.GetById(It.IsAny<int>()))
                .Returns(new Email() { EmailAddress = "qq@qq.qq" });
            mockMapper.Setup(m => m.Map<EmailRecipientViewModel>(It.IsAny<EmailRecipient>()))
                .Returns(value: null);
            Assert.Throws<NullReferenceException>(() => manager.GetEmailRecipientById(10));
        }

        [Test]
        public void GetEmailRecipients_GetEmailRecipients_ReturnListOfEmailRecipients()
        {
            List<EmailRecipient> recipients = new List<EmailRecipient>();
            recipients.Add(new EmailRecipient()
            {
                BirthDate = DateTime.Now,
                CompanyId = 1,
                EmailId = 10,
                Gender = 0,
                Id = 1,
                IsSend = 0,
                KeyWords = "test",
                Name = "test",
                Priority = "Low",
                Surname = "test"
            });
            recipients.Add(new EmailRecipient()
            {
                BirthDate = DateTime.Now,
                CompanyId = 1,
                EmailId = 11,
                Gender = 0,
                Id = 1,
                IsSend = 0,
                KeyWords = "test",
                Name = "test",
                Priority = "Low",
                Surname = "test"
            });
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
            List<EmailRecipientViewModel> recipientViewModels = new List<EmailRecipientViewModel>();
            recipientViewModels.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "qq@qq.qq",
                Id = 1,
                Name = "test",
                BirthDate = DateTime.Now,
                Surname = "test",
                Gender = "Male",
                KeyWords = "test",
                Priority = "Low",
                CompanyId = 1
            });
            recipientViewModels.Add(new EmailRecipientViewModel()
            {
                EmailAddress = "tt@tt.tt",
                Id = 1,
                Name = "test",
                BirthDate = DateTime.Now,
                Surname = "test",
                Gender = "Male",
                KeyWords = "test",
                Priority = "Low",
                CompanyId = 1
            });
            mockUnitOfWork.Setup(u => u.EmailRecipients.Get(It.IsAny<Expression<Func<EmailRecipient, bool>>>(), It
                .IsAny<Func<IQueryable<EmailRecipient>,
                    IOrderedQueryable<EmailRecipient>>>(), It.IsAny<string>())).Returns(recipients);
            mockUnitOfWork.Setup(u => u.Emails.GetById(10)).Returns(Emails[0]);
            mockUnitOfWork.Setup(u => u.Emails.GetById(11)).Returns(Emails[1]);
            mockMapper.Setup(m =>
                m.Map<IEnumerable<EmailRecipient>, List<EmailRecipientViewModel>>(
                    It.IsAny<IEnumerable<EmailRecipient>>())).Returns(recipientViewModels);
            Assert.That(manager.GetEmailRecipients(1), Is.SameAs(recipientViewModels));
        }

        [Test]
        public void GetEmailRecipients_WithoutEmailRecipientsInDatabase_ThrowException ()
        {            
            mockUnitOfWork.Setup(u => u.EmailRecipients.Get(It.IsAny<Expression<Func<EmailRecipient, bool>>>(), It
                .IsAny<Func<IQueryable<EmailRecipient>,
                    IOrderedQueryable<EmailRecipient>>>(), It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<Exception>(() => manager.GetEmailRecipients(1));

        }

        [Test]
        public void Delete_DeleteEmailRecipient_DoesNotThrow()
        {
            mockUnitOfWork.Setup(u => u.EmailRecipients.GetById(It.IsAny<int>())).Returns(new EmailRecipient());
            mockUnitOfWork.Setup(u => u.EmailRecipients.Delete(It.IsAny<EmailRecipient>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Delete(1));
        }

        [Test]
        public void Delete_DeleteNotExistingEmailRecipient_ThrowNullReferenceException()
        {
            mockUnitOfWork.Setup(u => u.EmailRecipients.GetById(It.IsAny<int>())).Returns(value: null);
            mockUnitOfWork.Setup(u => u.EmailRecipients.Delete(null)).Throws<NullReferenceException>();
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<NullReferenceException>(() => manager.Delete(1));
        }
    }
}