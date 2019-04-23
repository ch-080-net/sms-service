using AutoMapper;
using Model.Interfaces;
using NUnit.Framework;
using Moq;

namespace BAL.Tests
{
	[TestFixture]
	public abstract class TestInitializer
	{
		protected static Mock<IUnitOfWork> mockUnitOfWork;
		protected static Mock<IMapper> mockMapper;

		[SetUp]
		protected virtual void Initialize()
		{
			mockUnitOfWork = new Mock<IUnitOfWork>();
			mockMapper = new Mock<IMapper>();
			TestContext.WriteLine("Initialize test data");
		}

		[TearDown]
		protected virtual void Cleanup()
		{
			TestContext.WriteLine("Cleanup test data");
		}

	}
}
