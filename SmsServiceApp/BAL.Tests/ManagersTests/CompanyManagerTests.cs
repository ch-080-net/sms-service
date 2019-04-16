using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BAL.Managers;
using BAL.Notifications.Infrastructure;
using BAL.Tests;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.EmailCampaignViewModels;
using Model.ViewModels.EmailRecipientViewModels;
using Model.ViewModels.GroupViewModels;
using Model.ViewModels.RecipientViewModels;
using Moq;
using NUnit.Framework;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public  class CompanyManagerTests : TestInitializer
    {
   
        private static Mock<INotificationsGenerator<Company>> mockNotification = new Mock<INotificationsGenerator<Company>>();
        private CompanyManager manager;


        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            manager = new CompanyManager(mockUnitOfWork.Object, mockMapper.Object, mockNotification.Object);      
        }

        [Test]
        public void Insert_EmptyObject_ReturnsFalse()
        {
            CompanyViewModel item = new CompanyViewModel();
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company());
            var result = manager.Insert(item);
            Assert.IsFalse(result);
        }

        [Test]
        public void Insert_NewObject_ReturnsTrue()
        {
            CompanyViewModel item = new CompanyViewModel() { Name = "Test", Description = "Test", PhoneId = 2, TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false};
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company() { Name = "Test", Description = "Test", PhoneId = 2, TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockUnitOfWork.Setup(u => u.Companies.Insert(new Company() { Name = "Test", Description = "Test", PhoneId = 2, TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false }));
            var result = manager.Insert(item);
            Assert.IsTrue(result);
        }

        [Test]
        public void Insert_ExistingObject_ReturnsTrue()
        {
            CompanyViewModel item = new CompanyViewModel() { Id = 1, Name = "Test", Description = "Test", PhoneId = 2, TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false };
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", PhoneId = 2, TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            var result = manager.Insert(item);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_EmptyCompany_ThrowsException()
        {
            CompanyViewModel item = new CompanyViewModel();
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company());
            Assert.Throws<NullReferenceException>(() => manager.Update(item));
        }

       

         [Test]
        public void Update_CompanyWithoutPhone_ReturnFalse()
        {
            CompanyViewModel item = new CompanyViewModel() { Id=1, Name = "Test", Description = "Test", TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false };
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company(){ Id =1, Name = "Test", Description = "Test", TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockUnitOfWork.Setup(u => u.Companies.GetById(1))
                .Returns(new Company(){Id=1, Name = "Test", Description = "Test", TariffId = 1, Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockUnitOfWork.Setup(u => u.Save()).Throws(new Exception());
            var result = manager.Update(item);
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_CompanyWithoutTariffId_ReturnFalse()
        {
            CompanyViewModel item = new CompanyViewModel() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false };
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test",  Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockUnitOfWork.Setup(u => u.Companies.GetById(1))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockUnitOfWork.Setup(u => u.Save()).Throws(new Exception());
            var result = manager.Update(item);
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_NotExistId_ReturnFalse()
        {
            mockUnitOfWork.Setup(u => u.Companies.GetById(0));
            var result = manager.Delete(0);
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_ExistingObject_ReturnTrue()
        {
            mockUnitOfWork.Setup(u => u.Companies.GetById(1))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            var result = manager.Delete(1);
            Assert.IsTrue(result);
        }

        [Test]
        public void Get_NotExistingId_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.Companies.GetById(0));
            var result = manager.Get(0);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_ExistingId_ReturnCompanyViewModel()
        {
            CompanyViewModel item = new CompanyViewModel() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false };
            mockUnitOfWork.Setup(u => u.Companies.GetById(1))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            var result = manager.Get(1);
            Assert.That(result, Is.EqualTo(result));
        }

        [Test]
        public void Insert_EmptyCampaign_FalseResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel();
            List<RecipientViewModel> recepientsList = new List<RecipientViewModel>();
            recepientsList.Add(new RecipientViewModel()
            {
                Phonenumber = "+380999999999",
                Birthdate = DateTime.Now,
                Gender = "Male",
                Name = "Test",
                Surname = "Test",
                Priority = "Low"
            });
            var result = manager.CreateWithRecipient(emptyCampaign, recepientsList);
            Assert.IsFalse(result);
        }

        //[Test]
        //public void CreateWithRecipient_CampignWithoutPhoneInDatabase_TrueResult()
        //{
        //    ManageViewModel emptyCampaign = new ManageViewModel() { PhoneId = 1, Name = "test", Description = "Test" };
        //    List<RecipientViewModel> recepientsList = new List<RecipientViewModel>();
        //    List<Phone> Phones = new List<Phone>();
        //    Phones.Add(
        //        new Phone()
        //        {
        //            Id = 10,
        //            PhoneNumber = "+380999999999"
        //        });
        //    Phones.Add(
        //        new Phone()
        //        {
        //            Id = 11,
        //            PhoneNumber = "+380999999998"
        //        });
        //    mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
        //        IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(new List<Phone>());
        //    mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
        //    mockUnitOfWork.Setup(u => u.Recipients.Insert(It.IsAny<Recipient>()));
        //    mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
        //    mockMapper.Setup(m => m.Map<Company>(It.IsAny<ManageViewModel>()))
        //        .Returns(new Company() { PhoneId = 1,Name = "test", Message = "test", Description = "Test" });
        //    recepientsList.Add(new RecipientViewModel()
        //    {
        //        Phonenumber = "+380999999997",
        //        Birthdate = DateTime.Now,
        //        Gender = "Male",
        //        Name = "John",
        //        Surname = "Snow",
        //        Priority = "Low"
        //    });
        //    mockMapper.Setup(m => m.Map<RecipientViewModel, Recipient>(It.IsAny<RecipientViewModel>()))
        //        .Returns(new Recipient() { PhoneId = 1, BirthDate = DateTime.Now, Name = "John", Surname = "Snow", Priority = "Low" });
        //    var result = manager.CreateWithRecipient(emptyCampaign, recepientsList);
        //    Assert.IsFalse(result);
        //}

       



    }
}
