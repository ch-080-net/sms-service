using BAL.Managers;
using Model.ViewModels.ContactViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        [Test]
        public void GetContact_InvalidData_EmptyResult()
        {
			mockUnitOfWork.Setup(m => m.Contacts.GetContactsByPageNumber(1,1, It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<Func<IQueryable<Contact>,
				IOrderedQueryable<Contact>>>())).Returns(new List<Contact>());

			mockMapper.Setup(c => c.Map<IEnumerable<Contact>, List<ContactViewModel>>(new List<Contact>()))
				.Returns(new List<ContactViewModel>());

			var result = manager.GetContact(-1, 1, 1);

			Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetContact_ValidData_ContactCollection()
        {
	        Contact contact = new Contact() { Name = "Name", Id = 10, PhoneId = 1};
	        mockUnitOfWork.Setup(m => m.Contacts.GetContactsByPageNumber(1, 1, It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<Func<IQueryable<Contact>,
		        IOrderedQueryable<Contact>>>())).Returns(new List<Contact>(){ contact });
	        mockUnitOfWork.Setup(m => m.Phones.GetById(1)).Returns(new Phone());

	        mockMapper.Setup(c => c.Map<IEnumerable<Contact>, List<ContactViewModel>>(It.IsAny<List<Contact>>()))
		        .Returns(new List<ContactViewModel>(){new ContactViewModel(){Name = "name"}});

	        var result = manager.GetContact(1, 1, 1);

	        Assert.That(result, Is.Not.Empty);
			Assert.That(result, Is.TypeOf<List<ContactViewModel>>());
        }

        [Test]
        public void GetContactBySearchValue_EmptyValue_EmptyResult()
        {
			List<Contact> testContactList = new List<Contact>()
                {
                    new Contact(){Name = "name"},
                    new Contact(){Name = "a"}

                };

			mockUnitOfWork.Setup(m => m.Contacts.GetAll()).Returns(testContactList);
			mockUnitOfWork.Setup(m => m.Phones.GetById(1)).Returns(new Phone() {PhoneNumber = "0"});
			mockMapper.Setup(c => c.Map<IEnumerable<Contact>, List<ContactViewModel>>(It.IsAny<List<Contact>>()))
				.Returns(new List<ContactViewModel>());

			var result = manager.GetContactBySearchValue(1,1,1,"");

			Assert.That(result, Is.Null);
        }

		[Test]
		public void GetContactBySearchValue_ValidSearchValue_NonEmptyCollection()
		{
			List<Contact> testContactList = new List<Contact>()
			{
				new Contact() { KeyWords = "name", ApplicationGroupId = 1, PhoneId = 1}, 
				new Contact() { KeyWords = "a", ApplicationGroupId = 1, PhoneId = 2}
			};

			mockUnitOfWork.Setup(m => m.Contacts.GetAll()).Returns(testContactList);
			mockUnitOfWork.Setup(m => m.Phones.GetById(1)).Returns(new Phone() { PhoneNumber = "+380993276748" });
			mockUnitOfWork.Setup(m => m.Phones.GetById(2)).Returns(new Phone() { PhoneNumber = "+380974357666" });
			mockMapper.Setup(c => c.Map<IEnumerable<Contact>, List<ContactViewModel>>(It.IsAny<IEnumerable<Contact>>()))
				.Returns(new List<ContactViewModel>(){new ContactViewModel()});

			var result = manager.GetContactBySearchValue(1, 1, 5, "name");

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		public void GetContactCount_InvalidId_NullResult()
		{
			mockUnitOfWork
				.Setup(m =>m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
				.Returns(new List<Contact>());

			var result = manager.GetContactCount(-1);

			Assert.That(result, Is.EqualTo(0));
		}

		[Test]
		public void GetContactCount_ValidData_CountAreMoreThanZero()
		{
			mockUnitOfWork
				.Setup(m => m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
				.Returns(new List<Contact>(){new Contact()});

			var result = manager.GetContactCount(1);

			Assert.That(result, Is.Not.EqualTo(0));
			Assert.That(result, Is.EqualTo(1));
		}

        [Test]
        public void GetContactCountBySearchValue_Value_CountEqualOne()
        {
            mockUnitOfWork
                .Setup(m => m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
                .Returns(new List<Contact>() { new Contact() {PhoneId = 1}});
            mockUnitOfWork.Setup(m => m.Phones.GetById(1)).Returns(new Phone(){PhoneNumber = "0"});
            var result = manager.GetContactCountBySearchValue(1,"0");

            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public void GetContactCountBySearchValue_EmptyValue_EmptyCountResult()
        {
            mockUnitOfWork
                .Setup(m => m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
                .Returns(new List<Contact>());
            mockUnitOfWork.Setup(m => m.Phones.GetById(1)).Returns(new Phone() { PhoneNumber = "0" });
            var result = manager.GetContactCountBySearchValue(1, "0");

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CreateContact_Value_ResultFalse()
        {
            List<Phone> testPhoneList = new List<Phone>()
            {
               new Phone(){PhoneNumber = "+380992033044"},
               new Phone(){PhoneNumber = "+380992033066"}
            };
            ContactViewModel contactViewModel = new ContactViewModel();
            List<Contact> tempContactList = new List<Contact>()
            {
                new Contact() {Id = 1, PhoneId = 1, Name = "Nick", ApplicationGroupId = 1}
            };
            mockMapper.Setup(c => c.Map<Contact>(It.IsAny<ContactViewModel>()))
               .Returns(new Contact());
            mockUnitOfWork.Setup(m => m.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
               .Returns(testPhoneList);
            mockUnitOfWork.Setup(m => m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
               .Returns(tempContactList);
            mockUnitOfWork.Setup(n => n.Save());
            mockUnitOfWork.Setup(n => n.Contacts.Insert(new Contact()));
            var result = manager.CreateContact(contactViewModel, 1);
            Assert.That(result,Is.EqualTo(false));

        }

        [Test]
        public void CreateContact_ValueNotEqual_ResultTrue()
        {
            List<Phone> testPhoneList = new List<Phone>()
            {
                new Phone(){PhoneNumber = "+380992033044"},
                new Phone(){PhoneNumber = "+380992033066"}
            };
            ContactViewModel contactViewModel = new ContactViewModel();
            List<Contact> tempContactList = new List<Contact>();
           
            mockMapper.Setup(c => c.Map<Contact>(It.IsAny<ContactViewModel>()))
                .Returns(new Contact());
            mockUnitOfWork.Setup(m => m.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
                .Returns(testPhoneList);
            mockUnitOfWork.Setup(m => m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
                .Returns(tempContactList);
            mockUnitOfWork.Setup(n => n.Save());
            mockUnitOfWork.Setup(n => n.Contacts.Insert(new Contact()));

            var result = manager.CreateContact(contactViewModel, 1);
            Assert.That(result, Is.EqualTo(true));

        }

        [Test]
        public void CreateContact_ValueCount_ResultTrue()
        {
            List<Phone> testPhoneList = new List<Phone>();

            ContactViewModel contactViewModel = new ContactViewModel()
            {
                PhonePhoneNumber = "+380992033044"
            };
            List<Contact> tempContactList = new List<Contact>();
          
            mockMapper.Setup(c => c.Map<Contact>(It.IsAny<ContactViewModel>()))
                .Returns(new Contact());
            mockUnitOfWork.Setup(m => m.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
                .Returns(testPhoneList);
            mockUnitOfWork.Setup(m=>m.Phones.Insert(new Phone()));
            mockUnitOfWork.Setup(m => m.Contacts.Get(It.IsAny<Expression<Func<Contact, bool>>>(), null, ""))
                .Returns(tempContactList);
            mockUnitOfWork.Setup(n => n.Save());

            var result = manager.CreateContact(contactViewModel, 1);
            Assert.That(result, Is.EqualTo(true));

        }

        [Test]
        public void DeleteContact_EmptyValue_Exseption()
        {
            Contact contact = new Contact();
            mockUnitOfWork.Setup(m => m.Contacts.GetById(1))
            .Returns(contact);
            mockUnitOfWork.Setup(m => m.Contacts.Delete(contact));
            mockUnitOfWork.Setup(n => n.Save()); 

            Assert.DoesNotThrow(() => manager.DeleteContact(1));

        }

        [Test]
        public void UpdateContact_ValueNotEqual_ResultTrue()
        {
            List<Phone> testPhoneList = new List<Phone>()
            {
               new Phone(){PhoneNumber = "+380992033044"},
               new Phone(){PhoneNumber = "+380992033066"}
            };
            Contact contact = new Contact();
            ContactViewModel contactViewModel = new ContactViewModel();
                    List<Contact> tempContactList = new List<Contact>()
            {
                new Contact() {Id = 1, PhoneId = 1, Name = "Nick", ApplicationGroupId = 1}
            };
            Phone phone = new Phone();
            mockMapper.Setup(c => c.Map<Contact>(It.IsAny<ContactViewModel>()))
               .Returns(new Contact());
            mockUnitOfWork.Setup(m => m.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
               .Returns(testPhoneList);
            mockUnitOfWork.Setup(m=>m.Phones.Insert(phone));
          

            mockUnitOfWork.Setup(m => m.Contacts.SetStateModified(contact));
            mockUnitOfWork.Setup(m => m.Contacts.Update(contact));
            mockUnitOfWork.Setup(n => n.Save());

            var result = manager.UpdateContact(contactViewModel, 1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void UpdateContact_ValueEqual_ResultTrue()
        {
            List<Phone> testPhoneList = new List<Phone>()
            {
            };
            Contact contact = new Contact();
            ContactViewModel contactViewModel = new ContactViewModel()
            {
                PhonePhoneNumber = "+380992033044"
            };
            List<Contact> tempContactList = new List<Contact>();

            mockMapper.Setup(c => c.Map<Contact>(It.IsAny<ContactViewModel>()))
                .Returns(new Contact());
            mockUnitOfWork.Setup(m => m.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
                .Returns(testPhoneList);
            mockUnitOfWork.Setup(m => m.Phones.Insert(new Phone()));

            mockUnitOfWork.Setup(m => m.Contacts.SetStateModified(contact));
            mockUnitOfWork.Setup(m => m.Contacts.Update(contact));
            mockUnitOfWork.Setup(n => n.Save());

            var result = manager.UpdateContact(contactViewModel, 1);
            Assert.That(result, Is.EqualTo(true));

        }
      

    }
}
