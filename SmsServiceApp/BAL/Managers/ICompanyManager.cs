using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public interface ICompanyManager
    {
        IEnumerable<CompanyViewModel> GetCompanies(string userId);
        void Insert(CompanyViewModel item, string userId);
        void Update(CompanyViewModel item, string userId);
        void Delete(int id);
    }
}
