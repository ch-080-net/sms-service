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
using System.Data;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Model.ViewModels.CodeViewModels;
using Model.DTOs;


namespace BAL.Test.ManagersTests
{
    [TestClass]
    public class MailingManagerTests
    {
        private static Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
        private static Mock<IMapper> mockMapper = new Mock<IMapper>();
        MailingManager manager = new MailingManager(mockUnitOfWork.Object, mockMapper.Object);
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
        public void GetUnsentMessages_NoValidMessages_EmptyEnumeration()
        {
            IEnumerable<Recipient> emptyEnumeration = new List<Recipient>();
            IEnumerable<MessageDTO> emptyDtoEnumeration = new List<MessageDTO>();
            mockUnitOfWork.Setup(m => m.Mailings.Get(It.IsAny<Expression<Func<Recipient, bool>>>(),
                    It.IsAny<Func<IQueryable<Recipient>,
                    IOrderedQueryable<Recipient>>>(), It.IsAny<string>()))
                .Returns(emptyEnumeration);
            mockMapper.Setup(m => m.Map<IEnumerable<Recipient>,
                IEnumerable<MessageDTO>>(It.Is<IEnumerable<Recipient>>(x => 
                x == emptyEnumeration))).Returns(emptyDtoEnumeration);

            var result = manager.GetUnsentMessages();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetUnsentMessages_NValidMessages_EnumerationWithNDTOs()
        {
            int n = 50;
            ICollection<Recipient> recipientEnumeration = new List<Recipient>();
            ICollection<MessageDTO> dtoEnumeration = new List<MessageDTO>();
            for (int i = 0; i < n; i++)
            {
                recipientEnumeration.Add(new Recipient());
                dtoEnumeration.Add(new MessageDTO());
            }
            mockUnitOfWork.Setup(m => m.Mailings.Get(It.IsAny<Expression<Func<Recipient, bool>>>(),
                    It.IsAny<Func<IQueryable<Recipient>,
                    IOrderedQueryable<Recipient>>>(), It.IsAny<string>()))
                .Returns(recipientEnumeration);            
            mockMapper.Setup(m => m.Map<IEnumerable<Recipient>,
                IEnumerable<MessageDTO>>(It.Is<IEnumerable<Recipient>>(x =>
                x == recipientEnumeration))).Returns(dtoEnumeration);

            var result = manager.GetUnsentMessages();

            Assert.IsTrue(result.Count() == n);
        }
    }
}
