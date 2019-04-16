using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.RecipientViewModels;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class RecipientManagerTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private RecipientManager manager;
        private Recipient item;
        private RecipientViewModel model;

        [SetUp]
        public void SetUp()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();
            manager = new RecipientManager(mockUnitOfWork.Object, mockMapper.Object);
            item = new Recipient() {Id = 1, PhoneId = 1, Name = "Test", CompanyId = 1};
            model = new RecipientViewModel() { Id = 1, Name = "Test", CompanyId = 1, Phonenumber = "+380661660777"};
        }

        [Test]
        public void Delete_NotExistingId_ReturnFalse()
        {
            mockUnitOfWork.Setup(u => u.Recipients.GetById(0));
            var result = manager.Delete(0);
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_ExistingId_ReturnTrue()
        {
            mockUnitOfWork.Setup(u => u.Recipients.GetById(1)).Returns(item);
            var result = manager.Delete(1);
            Assert.IsTrue(result);
        }

        [Test]
        public void GetRecipients_NotExistingId_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.Recipients.Get(It.IsAny<Expression<Func<Recipient, bool>>>(), null, ""))
                .Returns(new List<Recipient>());
            mockMapper.Setup(m => m.Map<IEnumerable<Recipient>, IEnumerable<RecipientViewModel>>(new List<Recipient>()))
                .Returns(new List<RecipientViewModel>());
            var result = manager.GetRecipients(2);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetRecipients_ExistingId_ReturnIEnumerable()
        {
            IEnumerable<Recipient> list = new List<Recipient>()
            {
                item
            };
            IEnumerable<RecipientViewModel> models = new List<RecipientViewModel>()
            {
                model
            };
            mockUnitOfWork.Setup(u => u.Recipients.Get(It.IsAny<Expression<Func<Recipient, bool>>>(), null, ""))
                .Returns(list);
            mockUnitOfWork.Setup(u => u.Phones.GetById(1))
                .Returns(new Phone(){Id = 1, PhoneNumber = "+380661660777"});
            mockMapper.Setup(m => m.Map<IEnumerable<Recipient>, IEnumerable<RecipientViewModel>>(list))
                .Returns(models);
            var result = manager.GetRecipients(1);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Insert_EmptyObject_ThrowException()
        {
            mockMapper.Setup(m => m.Map<RecipientViewModel, Recipient>(new RecipientViewModel()))
                .Returns(new Recipient());
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""));
            mockUnitOfWork.Setup(u => u.Recipients.Insert(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<NullReferenceException>(() => manager.Insert(model, 1));
        }
    }
}
