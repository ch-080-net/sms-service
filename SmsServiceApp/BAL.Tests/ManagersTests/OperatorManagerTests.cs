using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Models;
using System.Linq;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NUnit.Framework;
using NUnit.Framework.Internal;
using BAL.Wrappers;

namespace BAL.Tests.ManagersTests
{
	[TestFixture]
	public class OperatorManagerTests : TestInitializer
	{
		IOperatorManager manager;
        Mock<IFileIoWrapper> mockWrapper;

		[SetUp]
		protected override void Initialize()
		{
			base.Initialize();
            this.mockWrapper = new Mock<IFileIoWrapper>();
			manager = new OperatorManager(mockUnitOfWork.Object, mockMapper.Object, mockWrapper.Object);
			TestContext.WriteLine("Overrided");
		}

		[Test]
		public void GetAll_GettingOperators_SuccessResult()
		{
			List<Operator> operatorsList = new List<Operator>(){new Operator(){Name = "Name"}};
			List<OperatorViewModel> operatorsViewList = new List<OperatorViewModel>() { new OperatorViewModel() { Name = "Name" } };
			mockUnitOfWork.Setup(n => n.Operators.GetAll())
                .Returns(operatorsList);
			mockMapper.Setup(m => m.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>(new List<Operator>())).Returns(operatorsViewList);

			var result = manager.GetAll();

			Assert.That(result, Is.EqualTo(new List<OperatorViewModel>()));
		}

		[Test]
		public void GetByName_GettingOperators_SuccessResult()
		{
			Operator item = new Operator() { Name = "name" };
			OperatorViewModel itemView = new OperatorViewModel() { Name = "name" };
			List<Operator> operatorsList = new List<Operator>() { new Operator() { Name = "name" } };
			mockUnitOfWork.Setup(n => n.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(),null,""))
				.Returns(operatorsList);
			mockMapper.Setup(m => m.Map<OperatorViewModel>(It.IsAny<Operator>())).Returns(itemView);

			var result = manager.GetByName("name");

			Assert.That(result, Is.EqualTo(itemView));
		}

		[Test]
		public void GetById_GettingOperators_SuccessResult()
		{
			Operator item = new Operator() {Name = "name"};
			OperatorViewModel itemView = new OperatorViewModel() {Name = "name"};
			mockUnitOfWork.Setup(n => n.Operators.GetById(1)).Returns(item);
			mockMapper.Setup(m => m.Map<OperatorViewModel>(item)).Returns(itemView);

			var result = manager.GetById(1);

			Assert.That(result, Is.EqualTo(itemView));
		}

		[Test]
		public void Add_EmptyOperator_ErrorResult()
		{
			OperatorViewModel emptyOperator = new OperatorViewModel();

			var result = manager.Add(emptyOperator);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void Add_ExistingOperator_ErrorResult()
		{
			var testList = new List<Operator>() {new Operator(){Name = "name"}, new Operator(){Name = "ds"}};
			OperatorViewModel testOperator = new OperatorViewModel() {Name = "name"};
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(testList);

			var result = manager.Add(testOperator);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
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
			Assert.That(result.Success, Is.False);
		}

		[Test]
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
			Assert.That(result.Success, Is.True);
		}

		[Test]
		public void Remove_InvalidId_ErrorResult()
		{
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns((Operator)null);

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void Remove_OperatorWithTariffs_ErrorResult()
		{
			List<Tariff> tariffs = new List<Tariff>();
			tariffs.Add(new Tariff() { Name = "Tariff", Limit = 2, Price = 55 });
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns(new Operator() { Name ="name", Tariffs = tariffs});

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void Remove_OperatorWithoutTariffs_CatchExceptionError()
		{
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>()});
			mockUnitOfWork
				.Setup(n => n.Save()).Throws(new Exception());

			var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

        [Test]
        [TestCase(true)]
        [TestCase(false)]
		public void Remove_OperatorWithoutTariffs_SuccessResult(bool logoExists)
		{
			mockUnitOfWork
				.Setup(m => m.Operators.GetById(1))
				.Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>() });
			mockUnitOfWork
				.Setup(n => n.Operators.Delete(new Operator() { Name = "name" }));
			mockUnitOfWork
				.Setup(n => n.Save());
            mockWrapper.Setup(x => x.FileExists(It.IsAny<string>())).Returns(logoExists);

            var result = manager.Remove(1);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.True);
		}

        [Test]
        public void Remove_OperatorWithoutTariffsLogoRemovalFailed_ErrorResult()
        {
            mockUnitOfWork
                .Setup(m => m.Operators.GetById(1))
                .Returns(new Operator() { Name = "name", Tariffs = new List<Tariff>() });
            mockUnitOfWork
                .Setup(n => n.Operators.Delete(new Operator() { Name = "name" }));
            mockUnitOfWork
                .Setup(n => n.Save());
            mockWrapper.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockWrapper.Setup(x => x.FileDelete(It.IsAny<string>())).Throws(new Exception());

            var result = manager.Remove(1);

            TestContext.WriteLine(result.Details);
            Assert.That(result.Success, Is.False);
        }

        [Test]
		public void Update_EmptyOperator_ErrorResult()
		{
			OperatorViewModel test = new OperatorViewModel();

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void Update_OperatorWithoutName_ErrorResult()
		{
			OperatorViewModel test = new OperatorViewModel() { Name = "" };

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void Update_ExistingOperator_ErrorResult()
		{
			var testList = new List<Operator>() { new Operator() { Name = "name" }, new Operator() { Name = "ds" } };
			OperatorViewModel test = new OperatorViewModel() { Name = "name" };
			mockUnitOfWork
				.Setup(m => m.Operators.Get(It.IsAny<Expression<Func<Operator, bool>>>(), null, ""))
				.Returns(testList);

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
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
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void Update_OperatorObject_SuccessResult()
		{
			OperatorViewModel test = new OperatorViewModel() { Name = "name" };
			mockUnitOfWork
				.Setup(n => n.Operators.Update(new Operator() { Name = "name" }));
			mockUnitOfWork
				.Setup(n => n.Save());

			var result = manager.Update(test);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.True);
		}

		[Test]
		public void GetPage_Null_ReturnNull()
		{
			var result = manager.GetPage(null);

			Assert.IsNull(result);
		}

		[Test]
		public void GetPage_PageState_CurrentPage()
		{
			PageState test = new PageState() {  Page = 1};
			mockUnitOfWork
				.Setup(n => n.Operators.Get(null,null,""))
				.Returns(new List<Operator>() { new Operator() { Name = "tst" } });

			var result = manager.GetPage(test);

			Assert.IsNotNull(result);
		}

		[Test]
		public void AddLogo_EmptyLogo_ErrorResult()
		{
			LogoViewModel logo = new LogoViewModel() { Logo = null };

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void AddLogo_EmptyOperator_ErrorResult()
		{
			LogoViewModel logo = new LogoViewModel() {};

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void AddLogo_NullOperatorId_ErrorResult()
		{
			Mock<IFormFile> fileMock = new Mock<IFormFile>();
			LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object};

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void AddLogo_InvalidStream_CatchArgumentException()
		{
			Mock<IFormFile> fileMock = new Mock<IFormFile>();
			var ms = new MemoryStream();
			fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
			LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4};

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void AddLogo_LogoModel_CatchException()
		{
			Mock<IFormFile> fileMock = new Mock<IFormFile>();
			var ms = new MemoryStream();
			var image = 
			fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
			LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };

			var result = manager.AddLogo(logo);

			TestContext.WriteLine(result.Details);
			Assert.That(result.Success, Is.False);
		}

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddLogo_ValidLogoModelSaveSuccesfull_SuccessResult(bool directoryExistence)
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            var image = new Bitmap(100, 100);
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };
            mockWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(directoryExistence);

            var result = manager.AddLogo(logo);

            TestContext.WriteLine(result.Details);
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void AddLogo_ValidLogoModelDirectoryCreationFailed_ErrorResult()
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            var image = new Bitmap(100, 100);
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };
            mockWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockWrapper.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws(new Exception());

            var result = manager.AddLogo(logo);

            TestContext.WriteLine(result.Details);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void AddLogo_ValidLogoModelSaveFailedWithArgumentNullException_ErrorResult()
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            var image = new Bitmap(100, 100);
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };
            mockWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockWrapper.Setup(x => x.CreateDirectory(It.IsAny<string>()));
            mockWrapper.Setup(x => x.SaveBitmap(It.IsAny<Bitmap>(), It.IsAny<string>(), It.IsAny<ImageFormat>())).Throws(new ArgumentNullException());

            var result = manager.AddLogo(logo);

            TestContext.WriteLine(result.Details);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void AddLogo_ValidLogoModelSaveFailedWithExternalException_ErrorResult()
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            var image = new Bitmap(100, 100);
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };
            mockWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockWrapper.Setup(x => x.CreateDirectory(It.IsAny<string>()));
            mockWrapper.Setup(x => x.SaveBitmap(It.IsAny<Bitmap>(), It.IsAny<string>(), It.IsAny<ImageFormat>()))
                .Throws(new System.Runtime.InteropServices.ExternalException());

            var result = manager.AddLogo(logo);

            TestContext.WriteLine(result.Details);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void AddLogo_ValidLogoModelSaveFailedWithException_ErrorResult()
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            var image = new Bitmap(100, 100);
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            LogoViewModel logo = new LogoViewModel() { Logo = fileMock.Object, OperatorId = 4 };
            mockWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockWrapper.Setup(x => x.CreateDirectory(It.IsAny<string>()));
            mockWrapper.Setup(x => x.SaveBitmap(It.IsAny<Bitmap>(), It.IsAny<string>(), It.IsAny<ImageFormat>()))
                .Throws(new Exception());

            var result = manager.AddLogo(logo);

            TestContext.WriteLine(result.Details);
            Assert.That(result.Success, Is.False);
        }
    }
}
