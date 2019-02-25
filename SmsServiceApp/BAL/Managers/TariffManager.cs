using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class TariffManager: BaseManager, ITariffManager
    {
        public TariffManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        IEnumerable<TariffViewModel> ITariffManager.GetAll()
        {
            IEnumerable<Tariff> tariffs = unitOfWork.Tariffs.GetAll();
            return mapper.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(tariffs);
        }

        public void Insert(TariffViewModel item)
        {
            Tariff tariffs = mapper.Map<TariffViewModel, Tariff>(item);
            unitOfWork.Tariffs.Insert(tariffs);
            unitOfWork.Save();
        }

        
        public bool Update(TariffViewModel item)
        {
            var result = mapper.Map<Tariff>(item);
            try
            {
                unitOfWork.Tariffs.Update(result);
            }
            catch
            {
                return false;
            }
            unitOfWork.Save();
            return true;
        }

        public bool Delete(int Id)
        {
            var oper = unitOfWork.Tariffs.GetById(Id);
            if (oper == null)
                return false;
            try
            {
                unitOfWork.Tariffs.Delete(oper);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IEnumerable<TariffViewModel> GetPage(int Page = 1, int NumOfElements = 20)
        {
            var allTariffs= unitOfWork.Tariffs.GetAll();
            var operators = allTariffs.Skip(NumOfElements * (Page - 1)).Take(NumOfElements);
            var result = new List<TariffViewModel>();
            foreach (var o in operators)
            {
                result.Add(mapper.Map<TariffViewModel>(o));
            }
            return result;
        }

        public int GetNumberOfPages(int NumOfElements = 20)
        {
            return (unitOfWork.Operators.GetAll().Count() / NumOfElements) + 1;
        }

    }
}
