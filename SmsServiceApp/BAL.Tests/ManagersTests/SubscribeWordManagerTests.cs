﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BAL.Managers;
using Model.ViewModels.GroupViewModels;
using Model.ViewModels.SubscribeWordViewModels;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class SubscribeWordManagerTests : TestInitializer
    {
        private SubscribeWordManager manager;
        private SubscribeWord item;
        private SubscribeWordViewModel model;
        private CompanySubscribeWord companySubscribeWord;
        private List<CompanySubscribeWord> listWithItems;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            manager = new SubscribeWordManager(mockUnitOfWork.Object, mockMapper.Object);
            item = new SubscribeWord() {Id = 1, Word = "Test"};
            model = new SubscribeWordViewModel() {Id = 1, Word = "Test",CompanyId = 1};
            listWithItems = new List<CompanySubscribeWord>();
            companySubscribeWord = new CompanySubscribeWord() {CompanyId = 1, SubscribeWord = item, SubscribeWordId = 1};
            listWithItems.Add(companySubscribeWord);
            TestContext.WriteLine("Overrided");
        }

        [Test]
        public void GetWords_WordsNotExist_ReturnEmpty()
        {
            mockUnitOfWork.Setup(u => u.SubscribeWords.GetAll())
                .Returns(new List<SubscribeWord>());
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(new List<SubscribeWord>()))
                .Returns(new List<SubscribeWordViewModel>());
            var result = manager.GetWords();
            Assert.That(result, Is.Empty);
        }

    
        [Test]
        public void GetWordsByCompanyId_WordsNotExist_ReturnEmpty()
        {
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.GetAll())
                .Returns(new List<CompanySubscribeWord>());
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<CompanySubscribeWord>, IEnumerable<SubscribeWordViewModel>>(new List<CompanySubscribeWord>()))
                .Returns(new List<SubscribeWordViewModel>());
            var result = manager.GetWordsByCompanyId(0);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetWordsByCompanyId_WordsExist_ReturnList()
        {
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.GetAll())
                .Returns(listWithItems);
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(It.IsAny<Expression<Func<SubscribeWord, bool>>>(),
                It.IsAny<Func<IQueryable<SubscribeWord>,
                    IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>())).Returns(new List<SubscribeWord>() {item});
            mockMapper.Setup(m =>
                    m.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(new List<SubscribeWord>()
                        {item}))
                .Returns(new List<SubscribeWordViewModel>() { model });
            var result = manager.GetWordsByCompanyId(1);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Insert_EmptyObject_ThrowNullReferenceException()
        {
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(It.IsAny<Expression<Func<SubscribeWord, bool>>>(),
                It.IsAny<Func<IQueryable<SubscribeWord>,
                    IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>())).Returns(new List<SubscribeWord>());
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""));
            mockUnitOfWork.Setup(u => u.SubscribeWords.Insert(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<NullReferenceException>(() => manager.Insert(new SubscribeWordViewModel()));
        }

        [Test]
        public void Insert_NewCompanySubscribeWordWithoutSubscribeWordInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<SubscribeWordViewModel, SubscribeWord>(model))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(
                    It.IsAny<Expression<Func<SubscribeWord, bool>>>(),
                    It.IsAny<Func<IQueryable<SubscribeWord>,
                        IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>()))
                .Returns(new List<SubscribeWord>());
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.Insert(It.IsAny<CompanySubscribeWord>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(model));
        }

        [Test]
        public void Insert_NewCompanySubscribeWordWithSubscribeWordInDatabase_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<SubscribeWordViewModel, SubscribeWord>(model))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.SubscribeWords.Get(
                    It.IsAny<Expression<Func<SubscribeWord, bool>>>(),
                    It.IsAny<Func<IQueryable<SubscribeWord>,
                        IOrderedQueryable<SubscribeWord>>>(), It.IsAny<string>()))
                .Returns(new List<SubscribeWord>() {item});
            mockUnitOfWork.Setup(u => u.CompanySubscribeWords.Insert(It.IsAny<CompanySubscribeWord>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(model));
        }

        [Test]
        public void Update_EmptyObject_DoesNotThrow()
        {
            mockMapper.Setup(m => m.Map<SubscribeWordViewModel, SubscribeWord>(new SubscribeWordViewModel()))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.SubscribeWords.Update(It.IsAny<SubscribeWord>()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(model));
        }

        [Test]
        public void Update_NewSubscribeWordWithNotExistingPhone_Success()
        {
            mockMapper.Setup(m => m.Map<SubscribeWordViewModel, SubscribeWord>(model))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
                .Returns(new List<Phone>());
            mockUnitOfWork.Setup(u => u.SubscribeWords.Update(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(model));
        }

        [Test]
        public void Update_NewRecipientWithExistingPhone_ReturnTrue()
        {
            mockMapper.Setup(m => m.Map<SubscribeWordViewModel, SubscribeWord>(model))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.Phones.Get(It.IsAny<Expression<Func<Phone, bool>>>(), null, ""))
                .Returns(new List<Phone>()
                {
                    new Phone(){Id = 1, PhoneNumber = "+380661660777"}
                });
            mockUnitOfWork.Setup(u => u.SubscribeWords.Update(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(model));
        }

        [Test]
        public void Delete_NotExistingId_ThrowException()
        {
            mockUnitOfWork.Setup(u => u.SubscribeWords.GetById(0));
            mockUnitOfWork.Setup(u => u.SubscribeWords.Delete(new SubscribeWord()));
            mockUnitOfWork.Setup(u => u.Save()).Throws<Exception>();
            Assert.Throws<Exception>(() => manager.Delete(0));
        }

        [Test]
        public void Delete_ExistingId_Success()
        {
            mockUnitOfWork.Setup(u => u.SubscribeWords.GetById(1))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.SubscribeWords.Delete(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Delete(1));
        }
    }
}
