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
        bool Update(TariffViewModel item);
        bool Delete(int id);
        IEnumerable<TariffViewModel> GetAll();
        IEnumerable<TariffViewModel> GetPage(int Page = 1, int NumOfElements = 20);
        int GetNumberOfPages(int NumOfElements = 20);
       
    }
}
