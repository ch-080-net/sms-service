using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Interface with CRUD operation for Companies
    /// </summary>
    public interface ICompanyManager
    {
        CompanyViewModel Get(int id);
        IEnumerable<CompanyViewModel> GetCompanies(int groupId);
		int GetTariffLimit(int companyId);
		void Insert(CompanyViewModel item, int groupId);
		void Update(CompanyViewModel item, int groupId, int tariffId);
		void Delete(int id);
    }
}
