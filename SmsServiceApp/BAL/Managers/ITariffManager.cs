using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public interface ITariffManager
    {
        void Insert(TariffViewModel item);
        void Update(TariffViewModel item);
        void Delete(TariffViewModel item);
        IEnumerable<TariffViewModel> GetTariffs();
    }
}
