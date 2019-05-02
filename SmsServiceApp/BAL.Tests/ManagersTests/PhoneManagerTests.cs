using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAL.Managers;
using Model.ViewModels.GroupViewModels;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class PhoneManagerTests : TestInitializer
    {
        private PhoneManager manager;
        private Phone item;

        [SetUp]
        protected override void Initialize()
        {
            manager = new PhoneManager(mockUnitOfWork.Object, mockMapper.Object);
            item = new Phone() { Id = 1, PhoneNumber = "+380661660777"};
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void GetPhoneById_NotExistingId_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.Phones.GetById(0));
            var result = manager.GetPhoneById(0);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetPhoneById_ExistingId_ReturnPhone()
        {
            mockUnitOfWork.Setup(u => u.Phones.GetById(1))
                .Returns(item);
            var result = manager.GetPhoneById(1);
            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        public void GetPhoneNumber_NotExistingId_ThrowException()
        {
            mockUnitOfWork.Setup(u => u.Phones.GetById(0));
            Assert.Throws<NullReferenceException>(() => manager.GetPhoneNumber(0));
        }

        [Test]
        public void GetPhoneNumber_ExistingId_ReturnStringNumber()
        {
            mockUnitOfWork.Setup(u => u.Phones.GetById(1))
                .Returns(item);
            var result = manager.GetPhoneNumber(1);
            Assert.That(result, Is.EqualTo(item.PhoneNumber));
        }

        [Test]
        public void GetPhones_NoExistingPhones_ReturnEmpty()
        {
            mockUnitOfWork.Setup(u => u.Phones.GetAll())
                .Returns(new List<Phone>());
            var result = manager.GetPhones();
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetPhones_ExistingPhones_ReturnList()
        {
            mockUnitOfWork.Setup(u => u.Phones.GetAll())
                .Returns(new List<Phone>()
                {
                    item
                });
            var result = manager.GetPhones();
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Insert_EmptyObject_ThrowException()
        {
            mockUnitOfWork.Setup(u => u.Phones.Insert(new Phone()));
            mockUnitOfWork.Setup(u => u.Save()).Throws<ArgumentNullException>();
            Assert.Throws<ArgumentNullException>(() => manager.Insert(new Phone()));
        }

        [Test]
        public void Insert_NewPhone_Success()
        {
            mockUnitOfWork.Setup(u => u.Phones.Insert(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(item));
        }
    }
}
