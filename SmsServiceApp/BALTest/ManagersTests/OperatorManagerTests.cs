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
		public void Remove_InvalidId_ErrorResult()
		{
			mockUnitOfWork.Setup(m => m.Operators.GetById(1)).Returns((Operator)null);

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Remove_OperatorWithTariffs_ErrorResult()
		{
			List<Tariff> tariffs = new List<Tariff>();
			Tariff t = new Tariff() { Name = "Tariff", Limit = 2, Price = 55 };
			tariffs.Add(t);
			mockUnitOfWork.Setup(m => m.Operators.GetById(1)).Returns(new Operator() { Name ="name", Tariffs = tariffs});

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Remove_OperatorWithoutTariffs_CatchExceptionError()
		{
			mockUnitOfWork.Setup(m => m.Operators.GetById(1)).Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>()});
			mockUnitOfWork.Setup(n => n.Operators.Delete(new Operator() { Name = "name" })).Throws(new Exception());

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Remove_OperatorWithoutTariffs_SuccessResult()
		{
			mockUnitOfWork.Setup(m => m.Operators.GetById(1)).Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>() });
			mockUnitOfWork.Setup(n => n.Operators.Delete(new Operator() { Name = "name" }));
			mockUnitOfWork.Setup(n => n.Save());

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsTrue(result.Success);
		}

		[TestMethod]
		public void Update_EmptyOperator_ErrorResult()
		{
			OperatorViewModel test = new OperatorViewModel();

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Update_OperatorWithoutName_ErrorResult()
		{
			OperatorViewModel test = new OperatorViewModel() { Name = ""};

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}
	}
}
