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
        public IEnumerable<TariffViewModel> GetTariffs(int operatorId)
        {
            IEnumerable<Tariff> tariffs = unitOfWork.Tariffs.GetAll().Where(op => op.OperatorId == operatorId);
            
            return mapper.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(tariffs);
        }

        public TariffViewModel GetTariffById(int id)
        {
            Tariff tariff = unitOfWork.Tariffs.GetById(id);
            return mapper.Map<Tariff, TariffViewModel>(tariff);
        }

        IEnumerable<TariffViewModel> ITariffManager.GetAll()
        {
            IEnumerable<Tariff> tariffs = unitOfWork.Tariffs.GetAll();
            return mapper.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(tariffs);
        }
        public TariffViewModel GetById(int Id)
        {
            var tar = unitOfWork.Tariffs.GetById(Id);
            return mapper.Map<TariffViewModel>(tar);
        }
        public bool Insert(TariffViewModel item)
        {
            var check = unitOfWork.Tariffs.Get(o => o.Name == item.Name).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Tariff>(item);
            try
            {
                unitOfWork.Tariffs.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
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
        public void Delete(TariffViewModel item)
        {
            Tariff tariff = mapper.Map<TariffViewModel, Tariff>(item);
            unitOfWork.Tariffs.Delete(tariff);
            unitOfWork.Save();
        }
    }
}
