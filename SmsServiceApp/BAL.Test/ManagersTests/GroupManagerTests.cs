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

namespace BAL.Test.ManagersTests
{
    [TestFixture]
    public class GroupManagerTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private GroupManager manager;

        [SetUp]
        public void SetUp()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();
            manager = new GroupManager(mockUnitOfWork.Object, mockMapper.Object);
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
            GroupViewModel item = new GroupViewModel() { Id = 1, Name = "Test", PhoneId = 2 };
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup() { Id=1, Name = "Test", PhoneId = 2 });
            var result = manager.Insert(item);
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
            GroupViewModel item = new GroupViewModel(){Id = 1, Name = "Test"};
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup() { Id = 1, Name = "Test" });
            mockUnitOfWork.Setup(u => u.ApplicationGroups.GetById(1))
                .Returns(new ApplicationGroup() {Id = 1, Name = "Test"});
            mockUnitOfWork.Setup(u => u.Save()).Throws(new Exception());
            var result = manager.Update(item);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_ExistingGroup_ReturnTrue()
        {
            GroupViewModel item = new GroupViewModel(){Id =2, Name = "Test", PhoneId = 2};
            mockMapper.Setup(m => m.Map<GroupViewModel, ApplicationGroup>(item))
                .Returns(new ApplicationGroup() { Id = 2, Name = "Test", PhoneId = 2 });
            mockUnitOfWork.Setup(u =>
                u.ApplicationGroups.GetById(item.Id)).Returns(new ApplicationGroup()
                {
                    Id = 2, Name = "Test2", PhoneId = 2
                });
            var result = manager.Update(item);
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
                .Returns(new ApplicationGroup() {Id = 1, Name = "Test", PhoneId = 2});
            var result = manager.Delete(1);
            Assert.IsTrue(result);
        }

    }
}
