using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public interface ITariffManager
    {
        TariffViewModel GetById(int Id);
        bool Insert(TariffViewModel item);
        bool Update(TariffViewModel item);
        bool Delete(TariffViewModel item, int id);
        IEnumerable<TariffViewModel> GetAll();
        IEnumerable<TariffViewModel> GetTariffs(int operatorId);
        TariffViewModel GetTariffById(int id);

    }
}
