using AutoMapper;
using BAL.Interfaces;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.AdminStatisticViewModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class AdminStatisticManagerTest   : TestInitializer
    {
       
        private AdminStatisticManager _adminStatisticManager;
        private ApplicationGroup itemWithId;
        private AdminStatisticViewModel modelWithId;

        [SetUp]
        public void SetUp()
        {
            _adminStatisticManager = new AdminStatisticManager(mockUnitOfWork.Object, mockMapper.Object);

            itemWithId = new ApplicationGroup() { Id = 1, Name = "Test", PhoneId = 2 };
            modelWithId = new AdminStatisticViewModel() { GroupName = "Test",Count = 3};
        }
        [Test]
        public void CountNumberOfMessages_EmptyObject_ReturnIEnumerable()
        {
            List<ApplicationGroup> list = new List<ApplicationGroup>()
            {
                itemWithId
            };
            List<AdminStatisticViewModel> listModels = new List<AdminStatisticViewModel>()
            {
                modelWithId
            };

            mockUnitOfWork.Setup(u => u.AdminStatistics.GetAll()).Returns(list);
            mockMapper.Setup(m => m.Map<IEnumerable<ApplicationGroup>, IEnumerable<AdminStatisticViewModel>>(It.IsAny<IEnumerable<ApplicationGroup>>())).Returns(listModels);

            var result = _adminStatisticManager.NumberOfMessages();
            Assert.That(result, Is.EqualTo(listModels));
        }
    }
}
