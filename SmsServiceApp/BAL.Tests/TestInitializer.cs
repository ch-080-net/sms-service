using AutoMapper;
using Model.Interfaces;
using NUnit.Framework;
using Moq;

namespace BAL.Tests
{
	[TestFixture]
	public abstract class TestInitializer
	{
		protected static Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
		protected static Mock<IMapper> mockMapper = new Mock<IMapper>();
		protected TestContext TestContext { get; set; }

		[SetUp]
		protected virtual void Initialize()
		{
			TestContext.WriteLine("Initialize test data");
		}

		[TearDown]
		protected virtual void Cleanup()
		{
			TestContext.WriteLine("Cleanup test data");
		}

	}
}
