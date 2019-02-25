using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public interface ICompanyManager
    {
        CompanyViewModel Get(int id);
        IEnumerable<CompanyViewModel> GetCompanies(string userId);
        void Insert(CompanyViewModel item, string userId);
        void Update(CompanyViewModel item, string userId, int tariffId);
        void Delete(int id);
    }
}
