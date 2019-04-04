using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;
using Model.ViewModels.CampaignReportingViewModels;

namespace BAL.Managers
{
    public interface IChartsManager
    {
        CampaignDetailsViewModel GetChart(CampaignDetailsViewModel campaignDetails, string userId);
    }
}
