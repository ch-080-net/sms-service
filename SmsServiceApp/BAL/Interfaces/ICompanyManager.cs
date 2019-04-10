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
        int InsertWithId(StepViewModel item);
        void CreateWithRecipient(ManageViewModel item, List<RecipientViewModel> recipientList);
        int InsertWithId(CompanyViewModel item);
        void CreateCampaignCopy(ManageViewModel item);
        void Insert(CompanyViewModel item);
		void Update(CompanyViewModel item);
		void Delete(int id);
        void AddSend(SendViewModel item);
        void AddRecieve(RecieveViewModel item);
        void AddSendRecieve(SendRecieveViewModel item);
        ManageViewModel GetDetails(int id);
    }
}
