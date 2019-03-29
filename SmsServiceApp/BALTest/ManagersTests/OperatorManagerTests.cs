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

namespace BAL.Test.ManagersTests
{
	[TestClass]
	public class OperatorManagerTests
	{
		private static Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
		private static Mock<IMapper> mockMapper = new Mock<IMapper>();
		OperatorManager manager = new OperatorManager(mockUnitOfWork.Object, mockMapper.Object);
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
		public void Add_EmptyOperator_ErrorResult()
		{
			OperatorViewModel emptyOperator = new OperatorViewModel();

			var result = manager.Add(emptyOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Add_ExistingObject_ErrorResult()
		{
			OperatorViewModel testOperator = new OperatorViewModel();
			mockUnitOfWork.Setup(m => m.Operators.Get(n => n.Name == "derfk", null, "sdkj")).Returns(new List<Operator>() { new Operator() });

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Add_TestObject_CatchExceptionError()
		{
			OperatorViewModel testOperator = new OperatorViewModel() { Name = "Operator" };
			mockUnitOfWork.Setup(m => m.Operators.Get(n => n.Name == "derfk", null, "sdkj")).Returns(new List<Operator>());
			mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Add_NewTestObject_SuccessResult()
		{
			OperatorViewModel testOperator = new OperatorViewModel() { Name = "Operator" };
			mockUnitOfWork.Setup(m => m.Operators.Get(n => n.Name == "derfk", null, "sdkj")).Returns(new List<Operator>());
			mockUnitOfWork.Setup(n => n.Save());

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsTrue(result.Success);
		}

		[TestMethod]
		public void Remove_ValidId_SuccessResult()
		{
			mockUnitOfWork.Setup(m => m.Operators.GetById(1)).Returns(new Operator() { Name = "name", Tariffs = null});

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsTrue(result.Success);
		}
	}
}
