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
using System.IO;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

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
		public void Add_ExistingOperator_ErrorResult()
		{
			var testList = new List<Operator>() {new Operator(){Name = "name"}, new Operator(){Name = "ds"}};
			OperatorViewModel testOperator = new OperatorViewModel() {Name = "name"};
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(testList);

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Add_TestObject_CatchExceptionError()
		{
			OperatorViewModel testOperator = new OperatorViewModel() { Name = "Operator" };
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(new List<Operator>());
			mockUnitOfWork
				.Setup(n => n.Save()).Throws(new Exception());

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Add_NewTestObject_SuccessResult()
		{
			OperatorViewModel testOperator = new OperatorViewModel() { Name = "Operator" };
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(new List<Operator>());
			mockUnitOfWork
				.Setup(n => n.Save());

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.IsTrue(result.Success);
		}

		[TestMethod]
		public void Remove_InvalidId_ErrorResult()
		{
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns((Operator)null);

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Remove_OperatorWithTariffs_ErrorResult()
		{
			List<Tariff> tariffs = new List<Tariff>();
			tariffs.Add(new Tariff() { Name = "Tariff", Limit = 2, Price = 55 });
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns(new Operator() { Name ="name", Tariffs = tariffs});

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Remove_OperatorWithoutTariffs_CatchExceptionError()
		{
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>()});
			mockUnitOfWork
				.Setup(n => n.Save()).Throws(new Exception());

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Remove_OperatorWithoutTariffs_SuccessResult()
		{
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>() });
			mockUnitOfWork
				.Setup(n => n.Operators.Delete(new Operator() { Name = "name" }));
			mockUnitOfWork
				.Setup(n => n.Save());

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
			OperatorViewModel test = new OperatorViewModel() { Name = "" };

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Update_ExistingOperator_ErrorResult()
		{
			var testList = new List<Operator>() { new Operator() { Name = "name" }, new Operator() { Name = "ds" } };
			OperatorViewModel test = new OperatorViewModel() { Name = "name" };
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(testList);

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Update_OperatorObject_ErrorResult()
		{
			OperatorViewModel test = new OperatorViewModel() { Name = "name" };
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(new List<Operator>());
			mockUnitOfWork
				.Setup(n => n.Operators.Update(new Operator() { Name = "name"}));
			mockUnitOfWork
				.Setup(n => n.Save()).Throws(new Exception());

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Update_OperatorObject_SuccessResult()
		{
			OperatorViewModel test = new OperatorViewModel() { Name = "name" };
			mockUnitOfWork
				.Setup(n => n.Operators.Update(new Operator() { Name = "name" }));
			mockUnitOfWork
				.Setup(n => n.Save());

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.IsTrue(result.Success);
		}

		[TestMethod]
		public void GetPage_Null_ReturnNull()
		{
			var result = manager.GetPage(null);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetPage_PageState_CurrentPage()
		{
			PageState test = new PageState() {  Page = 1};
			mockUnitOfWork
				.Setup(n => n.Operators.Get(null,null,""))
				.Returns(new List<Operator>() { new Operator() { Name = "tst" } });

			var result = manager.GetPage(test);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void AddLogo_EmptyLogo_ErrorResult()
		{
			LogoViewModel logo = new LogoViewModel() { Logo = null };

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void AddLogo_EmptyOperator_ErrorResult()
		{
			LogoViewModel logo = new LogoViewModel() {};

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void AddLogo_NullOperatorId_ErrorResult()
		{
			Mock<IFormFile> fileMock = new Mock<IFormFile>();
			LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object};

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void AddLogo_InvalidStream_CatchArgumentException()
		{
			Mock<IFormFile> fileMock = new Mock<IFormFile>();
			var ms = new MemoryStream();
			fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
			LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4};

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void AddLogo_LogoModel_CatchException()
		{
			Mock<IFormFile> fileMock = new Mock<IFormFile>();
			var content = "Hello World from a Fake File";
			var fileName = "test.pdf";
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			writer.Write(content);
			writer.Flush();
			ms.Position = 0;
			fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
			fileMock.Setup(_ => _.FileName).Returns(fileName);
			fileMock.Setup(_ => _.Length).Returns(ms.Length);

			LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };

			var result = manager.AddLogo(logo); //bad work

			TestContext.WriteLine(result.Details);
			Assert.IsFalse(result.Success);
		}

	}
}
