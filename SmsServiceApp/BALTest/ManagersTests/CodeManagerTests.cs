using AutoMapper;
using BAL.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Models;
using System.Linq;
using System;
using Model.ViewModels.CodeViewModels;

namespace BAL.Test.ManagersTests
{
    [TestClass]
    public class CodeManagerTests
    {
        private static Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
        private static Mock<IMapper> mockMapper = new Mock<IMapper>();
        ICodeManager manager = new CodeManager(mockUnitOfWork.Object, mockMapper.Object);
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
        public void Add_EmptyCode_ErrorResult()
        {
            var emptyCode = new CodeViewModel();

            var result = manager.Add(emptyCode);

            TestContext.WriteLine(result.Details);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Add_ExistingCode_ErrorResult()
        {
            var testingCode = new CodeViewModel() {OperatorCode = "+380"};   
            mockUnitOfWork.Setup(m => m.Codes.Get(null, null, "")).Returns(new List<Code> { new Code() });

            var result = manager.Add(testingCode);

            TestContext.WriteLine(result.Details);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Add_CodeWithoutOperatorId_ErrorResult()
        {
            var newCode = new CodeViewModel() {OperatorCode = "+380"};

            var result = manager.Add(newCode);

            TestContext.WriteLine(result.Details);
            Assert.IsTrue(result.Success);
        }

    }
}
