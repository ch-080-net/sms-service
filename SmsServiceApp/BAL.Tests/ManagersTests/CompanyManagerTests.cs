﻿using System;
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
using Model.ViewModels.TariffViewModels;
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
        public void Update_CompanyWithoutId_ReturnFalse()
        {
            CompanyViewModel item = new CompanyViewModel() { Id = 1, Name = "Test",Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false };
            mockMapper.Setup(m => m.Map<CompanyViewModel, Company>(item))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockUnitOfWork.Setup(u =>
                u.Companies.GetById(It.IsAny<int>())).Returns(new Company());
            mockUnitOfWork.Setup(u => u.Save());
            var result = manager.Update(item);
            Assert.IsTrue(result);
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
        public void GetCompanies_List_ReturnList()
        {
            List<Company> campaigns = new List<Company>();
            campaigns.Add(
                new Company()
                {
                    Name = "test",
                    Description = "test",
                    PhoneId = 1,
                    Id = 1,
                }
            );
            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>(), It
                .IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(), It.IsAny<string>())).Returns(campaigns);
            var result = manager.GetCompanies(1);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetCompaniesByPhoneId_EmptyList_ReturnEmpty()
        {
            List<Company> campaigns = new List<Company>();
            campaigns.Add(
                new Company()
                {
                    Name = "test",
                    Description = "test",
                    PhoneId = 1,
                    Id = 1,
                }
            );
            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>(), It
                .IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(), It.IsAny<string>())).Returns(campaigns);
            var result = manager.GetCompaniesByPhoneId(1);
            Assert.That(result, Is.Empty);
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
        public void GetDetails_NotExistingId_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.Companies.GetById(0));
            var result = manager.GetDetails(0);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetDetails_ExistingId_ReturnManageViewModel()
        {
            ManageViewModel item = new ManageViewModel() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2 };
            mockUnitOfWork.Setup(u => u.Companies.GetById(1))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(item))
                .Returns(new Company() { Id = 1, Name = "Test", Description = "Test", Type = CompanyType.Send, ApplicationGroupId = 2, IsPaused = false });
            var result = manager.GetDetails(1);
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

        [Test]
        public void GetCampaignsCount_GetCampaignCount_IntResult()
        {
            List<Company> campaigns = new List<Company>();
            campaigns.Add(
                new Company()
                {
                    Name = "test",
                    Description = "test",
                    PhoneId = 1,
                    Id = 1,
                }
            );
            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>(), It
                .IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(), It.IsAny<string>())).Returns(campaigns);
            var result = manager.GetCampaignsCount(1, It.IsAny<string>());
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetCampaigns_WrongUser_NullResult()
        {
            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>(), It
                .IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(), It.IsAny<string>())).Returns(new List<Company>());
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<Company>, List<Company>>(
                        It.IsAny<IEnumerable<Company>>()))
                .Returns(value: null);
            var result = manager.GetCampaigns(1, 1, 1, "");
            Assert.IsNull(result);
        }

        [Test]
        public void GetCampaigns_RightUser_ListOfCamapaignsResult()
        {
            List<Company> campaigns = new List<Company>();
            campaigns.Add(
                new Company()
                {
                    Name = "test",
                    Description = "test",
                    PhoneId = 1,
                    Id = 1,
                }
            );
            List<CompanyViewModel> campaignsViewModels = new List<CompanyViewModel>();
            campaignsViewModels.Add(
                new CompanyViewModel()
                {
                    Name = "test",
                    Description = "test",
                    PhoneId = 1,
                    Id = 1,
                }
            );
            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>(), It
                .IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(), It.IsAny<string>())).Returns(campaigns);
            mockUnitOfWork.Setup(m => m.Companies.GetById(It.IsAny<int>()));
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<Company>, List<CompanyViewModel>>(
                        It.IsAny<IEnumerable<Company>>()))
                .Returns(campaignsViewModels);
            var result = manager.GetCampaigns(1, 1, 1, "");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTariffLimit_GetTariffLimit_IntResult()
        {
            List<Company> campaigns = new List<Company>();
            campaigns.Add(
                new Company()
                {
                    Name = "test",
                    Description = "test",
                    PhoneId = 1,
                    Id = 1,
                    TariffId = 1
                }
            );


            mockUnitOfWork.Setup(t => t.Tariffs.GetById(It.IsAny<int>())).Returns(new Tariff() {Limit = 1});
            mockUnitOfWork.Setup(m => m.Companies.Get(It.IsAny<Expression<Func<Company, bool>>>(), It
                .IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(), It.IsAny<string>())).Returns(campaigns);
            var result = manager.GetTariffLimit(1);
            Assert.AreEqual(1, result);
        }



        [Test]
        public void CreateWithRecipient_CampignWithPhoneInDatabase_TrueResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { PhoneId = 1, Name = "test", Description = "Test" };
            List<RecipientViewModel> recepientsList = new List<RecipientViewModel>();
            List<Phone> Phones = new List<Phone>();
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999999"
                });
            Phones.Add(
                new Phone()
                {
                    Id = 11,
                    PhoneNumber = "+380999999998"
                });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(Phones);
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Recipients.Insert(It.IsAny<Recipient>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.Insert(It.IsAny<CompanySubscribeWord>()));
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(It.IsAny<Expression<Func<SubscribeWord, bool>>>(), It.IsAny<Func<IQueryable<SubscribeWord>,
                IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>())).Returns(new List<SubscribeWord>(){new SubscribeWord(){Id=3,Word = "start"}});
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { PhoneId = 1, Name = "test", Message = "test", Description = "Test" });
            recepientsList.Add(new RecipientViewModel()
            {
                Phonenumber = "+380999999997",
                Birthdate = DateTime.Now,
                Gender = "Male",
                Name = "John",
                Surname = "Snow",
                Priority = "Low"
            });
            mockMapper.Setup(m => m.Map<RecipientViewModel, Recipient>(It.IsAny<RecipientViewModel>()))
                .Returns(new Recipient() { PhoneId = 1, BirthDate = DateTime.Now, Name = "John", Surname = "Snow", Priority = "Low" });
            var result = manager.CreateWithRecipient(emptyCampaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateWithRecipient_CampignWithoutPhoneInDatabase_TrueResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { PhoneId = 1, Name = "test", Description = "Test" };
            List<RecipientViewModel> recepientsList = new List<RecipientViewModel>();
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.Insert(It.IsAny<CompanySubscribeWord>()));
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(It.IsAny<Expression<Func<SubscribeWord, bool>>>(), It.IsAny<Func<IQueryable<SubscribeWord>,
                IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>())).Returns(new List<SubscribeWord>() { new SubscribeWord() { Id = 3, Word = "start" } });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(new List<Phone>());
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Recipients.Insert(It.IsAny<Recipient>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { PhoneId = 1, Name = "test", Message = "test", Description = "Test" });
            recepientsList.Add(new RecipientViewModel()
            {
                Phonenumber = "+380999999997",
                Birthdate = DateTime.Now,
                Gender = "Male",
                Name = "Test",
                Surname = "Test",
                Priority = "Low"
            });
            mockMapper.Setup(m => m.Map<RecipientViewModel, Recipient>(It.IsAny<RecipientViewModel>()))
                .Returns(new Recipient() { PhoneId = 1, BirthDate = DateTime.Now, Name = "Test", Surname = "Test", Priority = "Low" });
            var result = manager.CreateWithRecipient(emptyCampaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateWithRecipient_CampignWithWrongRecipientMapping_FalseResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { PhoneId = 1, Name = "test", Description = "Test" };
            List<RecipientViewModel> recepientsList = new List<RecipientViewModel>();
            List<Phone> Phones = new List<Phone>();
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999999"
                });
            Phones.Add(
                new Phone()
                {
                    Id = 11,
                    PhoneNumber = "+380999999998"
                });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(new List<Phone>());
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Recipients.Insert(It.IsAny<Recipient>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { PhoneId = 1, Name = "test", Message = "test", Description = "Test" });
            recepientsList.Add(new RecipientViewModel()
            {
                Phonenumber = "+380999999997",
                Birthdate = DateTime.Now,
                Gender = "Male",
                Name = "Test",
                Surname = "Test",
                Priority = "Low"
            });

            var result = manager.CreateWithRecipient(emptyCampaign, recepientsList);
            Assert.IsFalse(result);
        }

        [Test]
        public void CreateWithRecipient_CampignWithTariffZeroInDatabase_TrueResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { TariffId = 0,PhoneId = 1, Name = "test", Description = "Test" };
            List<RecipientViewModel> recepientsList = new List<RecipientViewModel>();
            List<Phone> Phones = new List<Phone>();
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999999"
                });
            Phones.Add(
                new Phone()
                {
                    Id = 11,
                    PhoneNumber = "+380999999998"
                });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(Phones);
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.Insert(It.IsAny<CompanySubscribeWord>()));
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(It.IsAny<Expression<Func<SubscribeWord, bool>>>(), It.IsAny<Func<IQueryable<SubscribeWord>,
                IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>())).Returns(new List<SubscribeWord>() { new SubscribeWord() { Id = 3, Word = "start" } });
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Recipients.Insert(It.IsAny<Recipient>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { TariffId = 0, PhoneId = 1, Name = "test", Message = "test", Description = "Test" });
            recepientsList.Add(new RecipientViewModel()
            {
                Phonenumber = "+380999999997",
                Birthdate = DateTime.Now,
                Gender = "Male",
                Name = "John",
                Surname = "Snow",
                Priority = "Low"
            });
            mockMapper.Setup(m => m.Map<RecipientViewModel, Recipient>(It.IsAny<RecipientViewModel>()))
                .Returns(new Recipient() {PhoneId = 1, BirthDate = DateTime.Now, Name = "John", Surname = "Snow", Priority = "Low" });
            var result = manager.CreateWithRecipient(emptyCampaign, recepientsList);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateCompanyCopy_CampaignWithoutPhone_TrueResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { PhoneId = 1, Name = "test", Description = "Test" };
            List<Phone> Phones = new List<Phone>();
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999999"
                });
            Phones.Add(
                new Phone()
                {
                    Id = 11,
                    PhoneNumber = "+380999999998"
                });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(new List<Phone>());
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { PhoneId = 1, Name = "test", Message = "test", Description = "Test" });
            var result = manager.CreateCampaignCopy(emptyCampaign);
            Assert.IsTrue(result);
        }


        [Test]
        public void CreateCompanyCopy_CampaignWithZeroTariff_TrueResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { TariffId = 0, Name = "test", Message = "test", Description = "Test" };
            List<Phone> Phones = new List<Phone>();
          
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999999"
                });
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999998"
                });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(Phones);
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>()));
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { PhoneId = 1, TariffId = 0, Name = "test", Message = "test", Description = "Test" });
            var result = manager.CreateCampaignCopy(emptyCampaign);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateCompanyCopy_CampignWithWrongPhoneMapping_FalseResult()
        {
            ManageViewModel emptyCampaign = new ManageViewModel() { PhoneId = 1, Name = "test", Description = "Test" };
            List<Phone> Phones = new List<Phone>();
            Phones.Add(
                new Phone()
                {
                    Id = 10,
                    PhoneNumber = "+380999999999"
                });
            Phones.Add(
                new Phone()
                {
                    Id = 11,
                    PhoneNumber = "+380999999998"
                });
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), It.IsAny<Func<IQueryable<Phone>,
                IOrderedQueryable<Phone>>>(), It.IsAny<string>())).Returns(new List<Phone>());
            mockUnitOfWork.Setup(u => u.Companies.Insert(It.IsAny<Company>()));
            mockUnitOfWork.Setup(u => u.Phones.Insert(It.IsAny<Phone>())).Throws(new Exception());
            mockMapper.Setup(m => m.Map<ManageViewModel, Company>(It.IsAny<ManageViewModel>()))
                .Returns(new Company() { PhoneId = 1, Name = "test", Message = "test", Description = "Test" });
          

            var result = manager.CreateCampaignCopy(emptyCampaign);
            Assert.IsFalse(result);
        }

       

    }
}
