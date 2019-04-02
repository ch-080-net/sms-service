using AutoMapper;
using BAL.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Models;
using System.Linq;
using System;

namespace BAL.Test.ManagersTests
{
    [TestClass]
    public class TariffManagerTests
    {
        private static Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
        private static Mock<IMapper> mockMapper = new Mock<IMapper>();
        TariffManager manager = new TariffManager(mockUnitOfWork.Object, mockMapper.Object);
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            TestContext.WriteLine("Initialize test data");
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestContext.WriteLine($"Test name: {TestContext.TestName}");
            TestContext.WriteLine($"Test result: {TestContext.CurrentTestOutcome}");
            TestContext.WriteLine("Cleanup test data");
        }

        [TestMethod]
        public void Update_EmptyTariff_ErrorResult()
        {
            TariffViewModel emptyTariff = new TariffViewModel();

            var result = manager.Update(emptyTariff);

            if (result)
            {
                TestContext.WriteLine("result = true ?!");
            }
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void Update_ExistingObject_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel();
            //mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());
            mockUnitOfWork.Setup(n => n.Tariffs.Update(new Tariff() { Id=9,Name = "kjn", Limit=4,Price=5, Description="test" , OperatorId=4 }));
            mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());
            var result = manager.Update(testTariff);
            if (result)
            {
                TestContext.WriteLine("throw-catch sometimes it works incorrectly or Update does not make Exception");
            }
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Update_TariffWithout_SuccessResult()
        {
            TariffsViewModel testTariff = new TariffsViewModel();
            mockUnitOfWork.Setup(n => n.Tariffs.Update(new Tariff()));
            mockUnitOfWork.Setup(n => n.Save());
            var result = manager.Update(new TariffViewModel());
            if (result)
            {
                TestContext.WriteLine("?");
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Delete_EmptyTariff_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel();
            int testInt = 3;
            mockUnitOfWork.Setup(n => n.Tariffs.Delete(new Tariff() { Id = 9, Name = "kjn", Limit = 4, Price = 5, Description = "test", OperatorId = 4 }));
            mockUnitOfWork.Setup(n => n.Tariffs.GetById(2)).Returns((Tariff)null);
            mockUnitOfWork.Setup(n => n.Save());

            var result = manager.Delete(testTariff, testInt);
            if (result)
            {
                TestContext.WriteLine("?");
            }

            Assert.IsFalse(result);
        }

    }
}
