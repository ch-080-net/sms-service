using Model.ViewModels.CampaignReportingViewModels;

namespace BAL.Managers
{
    public interface IChartsManager
    {
        CampaignDetailsViewModel GetChart(CampaignDetailsViewModel campaignDetails, string userId);
    }
}
