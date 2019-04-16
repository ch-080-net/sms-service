using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Models;
using System.Linq;
using System;
using System.Linq.Expressions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BAL.Tests.ManagersTests
{
	[TestFixture]
	public class TariffManagerTests : TestInitializer
    {
        readonly ITariffManager manager = new TariffManager(mockUnitOfWork.Object, mockMapper.Object);
        private Tariff modelWithId;
        private TariffViewModel itemWithId;
        private IEnumerable<TariffViewModel> listModelWithId;
        ITariffManager manager;

        [SetUp]
        protected override void Initialize()
        {
	        base.Initialize();
	        manager = new TariffManager(mockUnitOfWork.Object, mockMapper.Object);
	        TestContext.WriteLine("Overrided");
     
            modelWithId =new Tariff() { Id = 9, Name = "kjn", Limit = 4, Price = 5, Description = "test", OperatorId = 4 };
            itemWithId = new TariffViewModel() { Id = 9, Name = "kjn", Limit = 4, Price = 5, Description = "test", OperatorId = 4 };
            listModelWithId = new List<TariffViewModel>() {itemWithId};
        }
        [Test]
        public void Update_ExistingObject_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel();
            //mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());
            mockUnitOfWork.Setup(n => n.Tariffs.Update(new Tariff() { Id=9,Name = "kjn", Limit=4,Price=5, Description="test" , OperatorId=4 }));
            mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());
            var result = manager.Update(testTariff);
          
           TestContext.WriteLine("throw-catch sometimes it works incorrectly or Update does not make Exception");
            
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_TariffWithout_SuccessResult()
        {
            TariffsViewModel testTariff = new TariffsViewModel();
            mockUnitOfWork.Setup(n => n.Tariffs.Update(new Tariff()));
            mockUnitOfWork.Setup(n => n.Save());
            var result = manager.Update(new TariffViewModel());
          
            Assert.IsTrue(result);
        }

        [Test]
        public void Delete_EmptyTariff_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel();
            int testInt = 3;
            mockUnitOfWork.Setup(n => n.Tariffs.Delete(modelWithId));
            mockUnitOfWork.Setup(n => n.Tariffs.GetById(2)).Returns((Tariff)null);
            mockUnitOfWork.Setup(n => n.Save());

            var result = manager.Delete(testTariff, testInt);
      
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_EmptyTariff_SuccessResult()
        {
            TariffViewModel testTariff = new TariffViewModel();

            mockUnitOfWork.Setup(n => n.Tariffs.GetById(9)).Returns(modelWithId);
            mockUnitOfWork.Setup(n => n.Tariffs.Delete(modelWithId));
            mockUnitOfWork.Setup(n => n.Save());
            var result = manager.Delete(testTariff, 9);

            Assert.IsTrue(result);
        }

        [Test]
        public void Delete_ExistingObject_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel();

            mockUnitOfWork.Setup(n => n.Tariffs.GetById(9)).Returns(new Tariff() { Id = 9, Name = "kjn", Limit = 4, Price = 5, Description = "test", OperatorId = 4 });
			mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());

            var result = manager.Delete(testTariff, 9);

            Assert.IsFalse(result);
        }


        [Test]
        public void Insert_EmptyTariffNotNull_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel(){Id=2};

            var testList = new List<Tariff> {
                new Tariff() {
                    Id = 2,
                    Description = "test",
                    Limit = 8,
                    Name = "testName",
                    Price = 5,
                    OperatorId = 4
                },
                new Tariff()
                {
                    Id=4
                }
            };

            mockUnitOfWork.Setup(n => n.Tariffs
                .Get(It.IsAny<Expression<Func<Tariff, bool>>>(), null, "")).Returns(testList);
            mockUnitOfWork.Setup(n => n.Save());
            var result = manager.Insert(testTariff);
        
            Assert.IsFalse(result);
        }

        [Test]
        public void Insert_EmptyTariffNull_SuccessResult()
        {
            TariffViewModel testTariff = new TariffViewModel();

            var tariff = mockUnitOfWork.Setup(n => n.Tariffs.Get(It.IsAny<Expression<Func<Tariff, bool>>>(), null, "")).Returns(new List<Tariff>(){});
            mockUnitOfWork.Setup(n => n.Save());
            var result = manager.Insert(testTariff);

            Assert.IsTrue(result);
        }

        [Test]
        public void Insert_SaveTariff_ErrorResult()
        {
            TariffViewModel testTariff = new TariffViewModel();
            var tariff = mockUnitOfWork.Setup(n => n.Tariffs.Get(It.IsAny<Expression<Func<Tariff, bool>>>(), null, "")).Returns(new List<Tariff>() { });

            mockUnitOfWork.Setup(n => n.Save()).Throws(new Exception());

            var result = manager.Insert(testTariff);

            Assert.IsFalse(result);
        }

        [Test]
        public void GetById_SuccessResult()
        { 
            mockUnitOfWork.Setup(n => n.Tariffs.GetById(9)).Returns(modelWithId);

            mockMapper.Setup(m => m.Map<TariffViewModel>(modelWithId))
                .Returns(itemWithId);

            var result = manager.GetById(9);

            Assert.That(result, Is.EqualTo(itemWithId));
        }

       
        [Test]
        public void GetTariffById_SuccessResult()
        {
            mockUnitOfWork.Setup(n => n.Tariffs.GetById(9)).Returns(modelWithId);

            mockMapper.Setup(m => m.Map<Tariff, TariffViewModel>(modelWithId))
                .Returns(itemWithId);

            var result = manager.GetTariffById(9);

            Assert.That(result, Is.EqualTo(itemWithId));
        }
        [Test]
        public void GetTariffs_SuccessResult()
        {
             mockUnitOfWork.Setup(n => n.Tariffs.Get(It.IsAny<Expression<Func<Tariff, bool>>>(), null, ""))
                 .Returns(new List<Tariff>() { modelWithId });
            
            mockMapper.Setup(m => m.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(new List<Tariff>())).Returns( listModelWithId );

            var result = manager.GetTariffs(4);
         
            Assert.That(result,Is.EqualTo(new List<TariffViewModel>()));

        }
    
        [Test]
        public void GetAll_SuccessResult()
        {
            mockUnitOfWork.Setup(n => n.Tariffs.GetAll())
                .Returns(new List<Tariff>() { modelWithId });
            mockMapper.Setup(m => m.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(new List<Tariff>())).Returns(listModelWithId);
            var result = manager.GetAll();

            Assert.That(result, Is.EqualTo(new List<TariffViewModel>()));
        }
    }
}
