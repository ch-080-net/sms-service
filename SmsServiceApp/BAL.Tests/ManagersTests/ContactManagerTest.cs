using BAL.Managers;
using Model.ViewModels.ContactViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class ContactManagerTest : TestInitializer
   {

       ContactManager manager;
        [SetUp]
       protected override void Initialize()
       {
           base.Initialize();
           manager = new ContactManager(mockUnitOfWork.Object, mockMapper.Object);
           TestContext.WriteLine("Overrided");
       }

       [Test]
       public void GetContact_NonExistingId_null()
       {
           Contact nullCode = null;
           mockUnitOfWork.Setup(m => m.Contacts.GetById(It.IsAny<int>())).Returns(nullCode);
           mockMapper.Setup(m => m.Map<ContactViewModel>(It.Is<Contact>(x => x != null))).Returns(new ContactViewModel());

           var result = manager.GetContact(43);

           Assert.IsNull(result);
       }

        [Test]
        public void GetContact_ExistingId_Contact()
        {
             var contact = new Contact();
            var contactViewModel = new ContactViewModel();
            int Id = 10;
            mockUnitOfWork.Setup(x => x.Contacts.GetById(It.Is<int>(c => c == Id))).Returns(contact);
            mockMapper.Setup(m => m.Map<ContactViewModel>(It.Is<Contact>(x => x == contact))).Returns(contactViewModel);
            var result = manager.GetContact(Id);

            Assert.AreEqual(contactViewModel, result);

        }

       
   }
}
