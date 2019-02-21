using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public interface ICompanyManager
    {
        IEnumerable<CompanyViewModel> GetCompanies();
        void Insert(CompanyViewModel item);
        void Update(CompanyViewModel item);
        void Delete(CompanyViewModel item);
        void SetStateModified(CompanyViewModel item);
    }
}
