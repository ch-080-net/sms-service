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
using System.Data;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Model.ViewModels.CodeViewModels;
using Model.DTOs;
using NUnit.Framework;


namespace BAL.Tests.ManagersTests
{
	[TestFixture]
	public class MailingManagerTests : TestInitializer
	{
		IMailingManager manager;

		[SetUp]
		protected override void Initialize()
		{
			base.Initialize();
			manager = new MailingManager(mockUnitOfWork.Object, mockMapper.Object);
			TestContext.WriteLine("Overrided");
		}

		[Test]
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

        [Test]
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

        [Test]
        public void MarkAs_MessageObject_ThrowsNothing()
        {
	        MessageDTO testMessage = new MessageDTO() { RecipientId = 1 };

	        mockUnitOfWork.Setup(m => m.Mailings.GetById(1)).Returns(new Recipient());
	        mockUnitOfWork.Setup(m => m.Save());

	        Assert.That(() => manager.MarkAs(testMessage, MessageState.Accepted), Throws.Nothing);

        }

		[Test]
        public void MarkAs_MessageCollection_ThrowsNothing()
        {
			MessageDTO testMessage = new MessageDTO() {RecipientId = 1};
			List<MessageDTO> testMessageList = new List<MessageDTO>(){testMessage};

			mockUnitOfWork.Setup(m => m.Mailings.GetById(1)).Returns(new Recipient());
			mockUnitOfWork.Setup(m => m.Save());

			Assert.That(() => manager.MarkAs(testMessageList, MessageState.Accepted), Throws.Nothing);

        }
    }
}
