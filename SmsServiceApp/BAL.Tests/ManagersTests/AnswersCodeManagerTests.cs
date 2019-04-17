using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.AnswersCodeViewModels;
using Model.ViewModels.RecipientViewModels;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
    [TestFixture]
    public class AnswersCodeManagerTests : TestInitializer
    {
        private AnswersCodeManager manager;
        private AnswersCode item;
        private AnswersCodeViewModel model;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            manager = new AnswersCodeManager(mockUnitOfWork.Object, mockMapper.Object);
            item = new AnswersCode() { Id = 1, Answer = "Test", Code = 1, CompanyId = 1};
            model = new AnswersCodeViewModel() { Id = 1, Answer = "Test", Code = 1, CompanyId = 1};
        }

        [Test]
        public void Delete_NotExistingId_ThrowException()
        {
            mockUnitOfWork.Setup(u => u.AnswersCodes.GetById(0));
            mockUnitOfWork.Setup(u => u.AnswersCodes.Delete(null));
            mockUnitOfWork.Setup(u => u.Save()).Throws<ArgumentNullException>();
            Assert.Throws<ArgumentNullException>(() => manager.Delete(0));
        }

        [Test]
        public void Delete_ExistingId_Success()
        {
            mockUnitOfWork.Setup(u => u.AnswersCodes.GetById(1))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.AnswersCodes.Delete(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Delete(1));
        }

        [Test]
        public void Insert_EmptyObject_ThrowException()
        {
            mockMapper.Setup(m => m.Map<AnswersCodeViewModel, AnswersCode>(new AnswersCodeViewModel()))
                .Returns(new AnswersCode());
            mockUnitOfWork.Setup(u => u.AnswersCodes.Insert(new AnswersCode()));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.Throws<NullReferenceException>(() => manager.Insert(new AnswersCodeViewModel(), 1));
        }

        [Test]
        public void Insert_NewAnswer_Success()
        {
            mockMapper.Setup(m => m.Map<AnswersCodeViewModel, AnswersCode>(model))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.AnswersCodes.Insert(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Insert(model, 1));
        }

        [Test]
        public void Update_EmptyObject_ThrowException()
        {
            mockMapper.Setup(m => m.Map<AnswersCodeViewModel, AnswersCode>(new AnswersCodeViewModel()))
                .Returns(new AnswersCode());
            mockUnitOfWork.Setup(u => u.AnswersCodes.Update(new AnswersCode()));
            mockUnitOfWork.Setup(u => u.Save()).Throws<NullReferenceException>();
            Assert.Throws<NullReferenceException>(() => manager.Update(new AnswersCodeViewModel()));
        }

        [Test]
        public void Update_NewAnswer_Success()
        {
            mockMapper.Setup(m => m.Map<AnswersCodeViewModel, AnswersCode>(model))
                .Returns(item);
            mockUnitOfWork.Setup(u => u.AnswersCodes.Update(item));
            mockUnitOfWork.Setup(u => u.Save());
            Assert.DoesNotThrow(() => manager.Update(model));
        }

        [Test]
        public void GetAnswersCodeById_NotExistingId_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.AnswersCodes.GetById(0))
                .Returns(new AnswersCode());
            mockMapper.Setup(m => m.Map<AnswersCodeViewModel>(new AnswersCode()))
                .Returns(new AnswersCodeViewModel());
            var result = manager.GetAnswersCodeById(0);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAnswersCodeById_ExistingId_ReturnViewModel()
        {
            mockUnitOfWork.Setup(u => u.AnswersCodes.GetById(1))
                .Returns(item);
            mockMapper.Setup(m => m.Map<AnswersCodeViewModel>(item))
                .Returns(model);
            var result = manager.GetAnswersCodeById(1);
            Assert.That(result, Is.EqualTo(model));
        }

        [Test]
        public void GetAnswersCodes_CompanyWithoutAnswers_ReturnNull()
        {
            mockUnitOfWork.Setup(u => u.AnswersCodes.Get(It.IsAny<Expression<Func<AnswersCode, bool>>>(), null, ""))
                .Returns(new List<AnswersCode>());
            mockMapper.Setup(m => m.Map<IEnumerable<AnswersCode>, List<AnswersCodeViewModel>>(new List<AnswersCode>()))
                .Returns(new List<AnswersCodeViewModel>());
            var result = manager.GetAnswersCodes(1);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAnswersCodes_CompanyWithAnswers_ReturnList()
        {
            mockUnitOfWork.Setup(u => u.AnswersCodes.Get(It.IsAny<Expression<Func<AnswersCode, bool>>>(), null, ""))
                .Returns(new List<AnswersCode>()
                {
                    item
                });
            mockMapper.Setup(m => m.Map<IEnumerable<AnswersCode>, IEnumerable<AnswersCodeViewModel>>(new List<AnswersCode>()
                {
                    item
                }))
                .Returns(new List<AnswersCodeViewModel>()
                {
                    model
                });
            var result = manager.GetAnswersCodes(1);
            Assert.That(result.Count(), Is.EqualTo(1));
        }
    }
}
