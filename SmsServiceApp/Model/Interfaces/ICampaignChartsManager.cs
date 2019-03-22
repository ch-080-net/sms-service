using Model.ViewModels.CampaingChartReportingViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interfaces
{
    public interface ICampaignChartsManager
    {
        CampaignChartViewModel GetChart(CampaignChartViewModel campaignDetails, string userId);

    }
}
