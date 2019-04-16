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
        bool Update(EmailCampaignViewModel item);
        bool Delete(int id);
        bool IncertWithRecepients(EmailCampaignViewModel campaign, List<EmailRecipientViewModel> emailRecipients);

    }
}
