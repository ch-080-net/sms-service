using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.CampaignReportingViewModels;
using Moq;
using NUnit.Framework;
using WebApp.Models;
using System.Linq;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class ChartsManagerTests : TestInitializer
    {
        private ChartsManager manager;

        [SetUp]
        protected override void Initialize()
        {
	        base.Initialize();
	        manager = new ChartsManager(mockUnitOfWork.Object, mockMapper.Object);
			TestContext.WriteLine("Overrided");
        }

        [Test]
        public void GetChart_EmptyCampaignDetails_null()
        {
            CampaignDetailsViewModel item = null;
            string userId = "NotNull";

            var result = manager.GetChart(item, userId);

            Assert.IsNull(result);
        }

        [Test]
        public void GetChart_nulluserId_null()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 1,
                CampaignName = "name",
                Selection = ChartSelection.MailingDetails
            };
            string userId = null;

            var result = manager.GetChart(item, userId);

            Assert.IsNull(result);
        }

        [Test]
        public void GetChart_EmptyUserId_null()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 1,
                CampaignName = "name",
                Selection = ChartSelection.MailingDetails
            };
            string userId = "";

            var result = manager.GetChart(item, userId);

            Assert.IsNull(result);
        }

        [Test]
        public void GetChart_NonExistingCampaignId_null()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "name",
                Selection = ChartSelection.MailingDetails
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company>());

            var result = manager.GetChart(item, userId);

            Assert.IsNull(result);
        }

        [Test]
        public void GetChart_IncorrectCampaignName_CorrectCampaignName()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "BadBoy!",
                Selection = ChartSelection.MailingDetails
            };
            Company comp = new Company
            {
                Name = "GoodBoy"
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { comp });

            var result = manager.GetChart(item, userId);

            StringAssert.AreEqualIgnoringCase(comp.Name, result.CampaignName);
        }

        [Test]
        public void GetChart_CompanyWithoutVoting_CorrectSelectionAndType()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "BadBoy!",
                Selection = ChartSelection.VotesDetails
            };
            Company comp = new Company
            {
                Name = "GoodBoy",
                Type = CompanyType.Send
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { comp });

            var result = manager.GetChart(item, userId);

            Assert.That(result.HaveVoting == false && result.Selection == ChartSelection.MailingDetails);
        }

        [Test]
        public void GetChart_CompanyWithVoting_CorrectType()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "BadBoy!",
                Selection = ChartSelection.VotesDetails
            };
            Company comp = new Company
            {
                Name = "GoodBoy",
                Type = CompanyType.SendAndRecieve
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { comp });

            var result = manager.GetChart(item, userId);

            Assert.IsTrue(result.HaveVoting);
        }

        [Test]
        public void GetChart_MailingDetailsSelection_NotNullCampaignPieChart()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "BadBoy!",
                Selection = ChartSelection.MailingDetails
            };
            Company comp = new Company
            {
                Name = "GoodBoy",
                Type = CompanyType.SendAndRecieve
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { comp });
            mockMapper.Setup(m => m.Map<Company, CompaingPieChart>(It.Is<Company>(x => x == comp))).Returns(new CompaingPieChart());

            var result = manager.GetChart(item, userId);

            Assert.NotNull(result.CompaingPieChart);
        }

        [Test]
        public void GetChart_VotesDetailsSelection_NotNullPieChart()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "BadBoy!",
                Selection = ChartSelection.VotesDetails
            };
            Company comp = new Company
            {
                Name = "GoodBoy",
                Type = CompanyType.SendAndRecieve
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { comp });
            mockMapper.Setup(m => m.Map<Company, PieChart>(It.Is<Company>(x => x == comp))).Returns(new PieChart());

            var result = manager.GetChart(item, userId);

            Assert.NotNull(result.PieChart);
        }

        [Test]
        public void GetChart_VotesDetailsByTimeSelection_NotNullCampaignPieChart()
        {
            CampaignDetailsViewModel item = new CampaignDetailsViewModel
            {
                CampaignId = 42,
                CampaignName = "BadBoy!",
                Selection = ChartSelection.VotesDetailsByTime
            };
            Company comp = new Company
            {
                Name = "GoodBoy",
                Type = CompanyType.SendAndRecieve
            };
            string userId = "WhoIsGoodBoy?";
            mockUnitOfWork.Setup(m => m.Charts.Get(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<Func<IQueryable<Company>,
                IOrderedQueryable<Company>>>(), It.IsAny<string>()))
                .Returns(new List<Company> { comp });
            mockMapper.Setup(m => m.Map<Company, StackedChart>(It.Is<Company>(x => x == comp))).Returns(new StackedChart());

            var result = manager.GetChart(item, userId);

            Assert.NotNull(result.StackedChart);
        }



    }
}
