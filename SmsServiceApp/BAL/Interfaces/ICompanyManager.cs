using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.RecipientViewModels;
using Model.ViewModels.StepViewModels;
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
        IEnumerable<CompanyViewModel> GetCompaniesByPhoneId(int phoneId);
        List<CompanyViewModel> GetCampaigns(int groupId, int page, int countOnPage, string searchValue);
        int GetCampaignsCount(int groupId, string searchValue);
        int GetTariffLimit(int companyId);
        void CreateWithRecipient(ManageViewModel item, List<RecipientViewModel> recipientList);
        void CreateCampaignCopy(ManageViewModel item);
        bool Insert(CompanyViewModel item);
		bool Update(CompanyViewModel item);
		bool Delete(int id);
        ManageViewModel GetDetails(int id);
    }
}
