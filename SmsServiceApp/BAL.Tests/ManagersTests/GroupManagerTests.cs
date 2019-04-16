using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.GroupViewModels;
using Moq;
using NUnit.Framework;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class GroupManagerTests : TestInitializer
    {
        private GroupManager manager;
        private ApplicationGroup itemWithId;
        private GroupViewModel modelWithId;

        [SetUp]
        protected override void Initialize()
        {
	        base.Initialize();
	        manager = new GroupManager(mockUnitOfWork.Object, mockMapper.Object);
	        itemWithId = new ApplicationGroup() { Id = 1, Name = "Test", PhoneId = 2 };
	        modelWithId = new GroupViewModel() { Id = 1, Name = "Test", PhoneId = 2 };
			TestContext.WriteLine("Overrided");
        }

        [Test]
        public void Insert_EmptyObject_ReturnsFalse()
        {
            GroupViewModel item = new GroupViewModel();
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup());
            var result = manager.Insert(item);
            Assert.IsFalse(result);

        }

        [Test]
        public void Insert_NewObject_ReturnsTrue()
        {
            GroupViewModel item = new GroupViewModel() {Name = "Test", PhoneId = 2};
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup() { Name = "Test", PhoneId = 2 });
            mockUnitOfWork.Setup(u => u.ApplicationGroups.Insert(new ApplicationGroup() {Name = "Test", PhoneId = 2}));
            var result = manager.Insert(item);
            Assert.IsTrue(result);
        }

        [Test]
        public void Insert_ExistingObject_ReturnsFalse()
        {
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(modelWithId))
                .Returns(itemWithId);
            var result = manager.Insert(modelWithId);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_EmptyGroup_ThrowsException()
        {
            GroupViewModel item = new GroupViewModel();
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup());
            Assert.Throws<NullReferenceException>(() => manager.Update(item));
        }

        [Test]
        public void Update_GroupWithoutPhone_ReturnFalse()
        {
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(modelWithId))
                .Returns(itemWithId);
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetById(1))
                .Returns(itemWithId);
            mockUnitOfWork.Setup(u => u.Save()).Throws(new Exception());
            var result = manager.Update(modelWithId);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_ExistingGroup_ReturnTrue()
        {
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(modelWithId))
                .Returns(itemWithId);
            mockUnitOfWork.Setup(u =>
                u.ApplicationGroups.GetById(itemWithId.Id)).Returns(itemWithId);
            var result = manager.Update(modelWithId);
            Assert.IsTrue(result);
        }

        [Test]
        public void Update_GroupWithoutId_ReturnFalse()
        {
            GroupViewModel item = new GroupViewModel() {Name = "", PhoneId = 2};
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup() {Name = "", PhoneId = 2});
            mockUnitOfWork.Setup(u =>
                u.ApplicationGroups.GetById(item.Id));
            var result = manager.Update(item);
            Assert.IsFalse(result);
        }


        [Test]
        public void Delete_NotExistId_ReturnFalse()
        {
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetById(0));
            var result = manager.Delete(0);
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_ExistingObject_ReturnTrue()
        {
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetById(1))
                .Returns(itemWithId);
            var result = manager.Delete(1);
            Assert.IsTrue(result);
        }

        [Test]
        public void Get_NotExistingId_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetById(0));
            var result = manager.Get(0);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_ExistingId_ReturnGroupViewModel()
        {
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetById(1))
                .Returns(itemWithId);
            mockMapper.Setup(m => m.Map<ApplicationGroup, GroupViewModel>(itemWithId))
                .Returns(modelWithId);
            var result = manager.Get(1);
            Assert.That(result, Is.EqualTo(modelWithId));
        }

        [Test]
        public void GetGroups_EmptyList_ReturnEmpty()
        {
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetAll());
            var result = manager.GetGroups();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetGroups_ExistingGroup_ReturnIEnumerable()
        {
            List<ApplicationGroup> list = new List<ApplicationGroup>()
            {
                itemWithId
            };
            List<GroupViewModel> listModels = new List<GroupViewModel>()
            {
               modelWithId
            };
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetAll())
                .Returns(list);
            mockMapper.Setup(m => m.Map<IEnumerable<ApplicationGroup>, IEnumerable<GroupViewModel>>(list))
                .Returns(listModels);
            var result = manager.GetGroups();
            Assert.That(result, Is.EqualTo(listModels));
        }
    }
}
