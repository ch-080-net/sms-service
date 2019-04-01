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
        public void Update_EmptyOperator_ErrorResult()
        {
            TariffViewModel emptyTariff = new TariffViewModel();

            var result = manager.Update(emptyTariff);

            if (result)
            {
                TestContext.WriteLine("result = true ?!");
            }
            Assert.IsFalse(result);
        }




    }
}
