using Model.ViewModels.EmailCampaignViewModels;
using Model.ViewModels.EmailRecipientViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IEmailCampaignManager
    {
        EmailCampaignViewModel GetById(int id);
        List<EmailCampaignViewModel> GetCampaigns(string userId, int page, int countOnPage, string searchValue);
        int GetCampaignsCount(string userId, string searchValue);
        void Insert(EmailCampaignViewModel item);
        int InsertWithId(EmailCampaignViewModel item);
        void Update(EmailCampaignViewModel item);
        void Delete(int id);
        void IncertWithRecepients(EmailCampaignViewModel campaign, List<EmailRecipientViewModel> emailRecipients);

    }
}
