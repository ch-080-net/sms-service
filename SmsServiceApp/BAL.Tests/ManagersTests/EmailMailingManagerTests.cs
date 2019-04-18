using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BAL.Managers;
using Model.DTOs;
using Moq;
using NUnit.Framework;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
	[TestFixture]
	public class EmailMailingManagerTests : TestInitializer
	{
		private EmailMailingManager manager;

		[SetUp]
		protected override void Initialize()
		{
			base.Initialize();
			manager = new EmailMailingManager(mockUnitOfWork.Object, mockMapper.Object);
		}

		[Test]
		public void GetUnsentEmails_EmailCollection_EmptyArray()
		{
			EmailCampaign ec = new EmailCampaign() { SendingTime = DateTime.MaxValue, Name = "name", EmailId = 2 };
			EmailRecipient item = new EmailRecipient() { CompanyId = 1 };

			mockUnitOfWork
				.Setup(u => u.EmailRecipients.Get(It.IsAny<Expression<Func<EmailRecipient, bool>>>(), null, ""))
				.Returns(new List<EmailRecipient>() { item });
			mockUnitOfWork.Setup(u => u.Emails.GetById(1)).Returns(new Email());
			mockUnitOfWork.Setup(u => u.EmailCampaigns.GetById(1)).Returns(ec);
			mockMapper
				.Setup(m => m.Map<IEnumerable<EmailRecipient>, IEnumerable<EmailDTO>>(new List<EmailRecipient>()))
				.Returns(new List<EmailDTO>(){new EmailDTO()});

			var result = manager.GetUnsentEmails();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		public void MarkAs_UndefinedRecipientId_ThrowsNothing()
		{
			mockUnitOfWork.Setup(m => m.EmailRecipients.GetById(1)).Returns(new EmailRecipient());
			mockUnitOfWork.Setup(m => m.Save());

			Assert.That(() => manager.MarkAs(new EmailDTO(), 1), Throws.Nothing);
		}

		[Test]
		public void MarkAs_MessageWithRecipientId_ChangeStateAndThrowsNothing()
		{
			mockUnitOfWork.Setup(m => m.EmailRecipients.GetById(1)).Returns(new EmailRecipient());
			mockUnitOfWork.Setup(m => m.Save());

			Assert.That(() => manager.MarkAs(new EmailDTO(){EmailRecipientId = 1}, 1), Throws.Nothing);
		}

	}
}
