using System;
using System.Collections.Generic;
using BAL.Managers;
using Moq;
using NUnit.Framework;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class EmailManagerTests: TestInitializer
    {
        private EmailManager manager;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            manager = new EmailManager(mockUnitOfWork.Object, mockMapper.Object);
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void IsEmailExist_ExistEmail_TrueResult()
        {
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
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Returns(Emails);
            var result = manager.IsEmailExist("qq@qq.qq");
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEmailExist_NotExistEmail_FalseResult()
        {
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
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Returns(Emails);
            var result = manager.IsEmailExist("q1@qq.qq");
            Assert.IsFalse(result);
        }

        [Test]
        public void IsEmailExist_GetErrorEmail_TrueResult()
        {
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Throws(new Exception("test"));
            var result = manager.IsEmailExist("");
            Assert.IsTrue(result);
        }

        [Test]
        public void Insert_Email_DoesNotThrow()
        {
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(new Email()));
        }

        [Test]
        public void Insert_WrongEmail_ThrowException()
        {
            mockUnitOfWork.Setup(u => u.Emails.Insert(It.IsAny<Email>())).Throws(new Exception("test"));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<Exception>(() => manager.Insert(new Email()));
        }

        [Test]
        public void GetEmails_AllEmail_ThrowNothing()
        {
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
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Returns(Emails);
            Assert.That(manager.GetEmails(), Is.SameAs(Emails));
        }

        [Test]
        public void GetEmails_AllEmail_ThrowExeption()
        {
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Throws(new Exception());
            Assert.Throws<Exception>(() => manager.GetEmails());
        }

        [Test]
        public void GetEmailId_NoEmailsInDatabase_ThrowExeption()
        {
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Returns(new List<Email>());
            Assert.Throws<NullReferenceException>(() => manager.GetEmailId("qq@qq.qq"));
        }

        [Test]
        public void GetEmailId_WithEmailsInDatabase_ReturnInt()
        {
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
            mockUnitOfWork.Setup(u => u.Emails.GetAll()).Returns(Emails);
            Assert.That(() => manager.GetEmailId("qq@qq.qq"), Is.TypeOf<int>());
        }

        [Test]
        public void GetEmailAddress_WithoutEmailsInDatabase_ReturnString()
        {
            mockUnitOfWork.Setup(u => u.Emails.GetById(10)).Returns(value: null);
            Assert.Throws<NullReferenceException>(() => manager.GetEmailAddress(10));
        }

        [Test]
        public void GetEmailAddress_WithEmailsInDatabase_ReturnInt()
        {
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
            mockUnitOfWork.Setup(u => u.Emails.GetById(10)).Returns(Emails[0]);
            Assert.That(() => manager.GetEmailAddress(10), Is.TypeOf<string>());
        }



        [Test]
        public void GetEmailById_WithEmailsInDatabase_ReturnEmail()
        {
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
            mockUnitOfWork.Setup(u => u.Emails.GetById(10)).Returns(Emails[0]);
            Assert.That(() => manager.GetEmailById(10), Is.TypeOf<Email>());
        }

        [Test]
        public void GetEmailById_WithoutEmailInDatabase()
        {
            mockUnitOfWork.Setup(u => u.Emails.GetById(10)).Throws<NullReferenceException>();
            Assert.Throws<NullReferenceException>(() => manager.GetEmailById(10));
        }
    }
}