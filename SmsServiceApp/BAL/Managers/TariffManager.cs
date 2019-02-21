using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class TariffManager: BaseManager, ITariffManager
    {
        public TariffManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        IEnumerable<TariffViewModel> ITariffManager.GetTariffs()
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

        public void Update(TariffViewModel item)
        {
            Tariff tariffs = mapper.Map<TariffViewModel, Tariff>(item);
            unitOfWork.Tariffs.Update(tariffs);
            unitOfWork.Save();
        }

        public void Delete(TariffViewModel item)
        {
            Tariff tariffs = mapper.Map<TariffViewModel, Tariff>(item);
            unitOfWork.Tariffs.Delete(tariffs);
            unitOfWork.Save();
        }

       
    }
}
