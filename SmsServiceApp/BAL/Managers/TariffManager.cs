using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class TariffManager: BaseManager
    {
        public TariffManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public IEnumerable<Tariff> GetTariffs()
        {
            return unitOfWork.Tariffs.GetAll();
        }
       public IEnumerable<Tariff> AddTariff()
        {
            Tariff tariff = new Tariff();
         
            return 
        }
    }
}
