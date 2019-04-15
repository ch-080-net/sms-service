using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Models;
using System.Linq;
using System;
using System.Linq.Expressions;
using BAL.Interfaces;
using Model.ViewModels.RecievedMessageViewModel;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BAL.Tests.ManagersTests
{
    public class RecievedMessageManagerTests : TestInitializer
    {
        IRecievedMessageManager recievedMessageManager = new RecievedMessageManager(mockUnitOfWork.Object, mockMapper.Object);

        private RecievedMessage message;
        private RecievedMessageViewModel viewMessage;
        private Phone phoneSender;
        private Phone phoneRecipient;
        [SetUp]
        public void SetUp()
        {
            phoneSender=new Phone(){Id=9,PhoneNumber = "+380999999999" };
            phoneRecipient = new Phone() { Id = 10, PhoneNumber = "+380111111111" };
            message = new RecievedMessage() {Id = 3,CompanyId = 1,PhoneId = 10,Message = "test"};
            viewMessage= new RecievedMessageViewModel(){SenderPhone = "+380999999999", RecipientPhone = "+380111111111", MessageText ="test"};
        }


        [Test]
        public void Get_SuccessResult()
        {
            mockUnitOfWork.Setup(n => n.RecievedMessages.GetById(3)).Returns( message );

            mockMapper.Setup(m => m.Map<RecievedMessage, RecievedMessageViewModel>(message)).Returns(viewMessage);


            var result = recievedMessageManager.Get(3);
            Assert.That(result,Is.EqualTo(viewMessage));
        }
    }
}
