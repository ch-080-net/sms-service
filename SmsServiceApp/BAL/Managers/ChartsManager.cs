using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using AutoMapper;
using Model.DTOs;
using System.Threading.Tasks;
using System.Linq;
using WebCustomerApp.Models;
using Model.ViewModels.CampaignReportingViewModels;

namespace BAL.Managers
{
    public class ChartsManager : BaseManager, IChartsManager
    {
        public ChartsManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public CampaignDetailsViewModel GetChart(CampaignDetailsViewModel campaignDetails, string userId)
        {
            if (campaignDetails == null || userId == null)
                return null;

            var campaign = unitOfWork.Charts.Get(pcc => pcc.Id == campaignDetails.CampaignId
                && pcc.ApplicationGroup.ApplicationUsers.Any(au => au.Id == userId)).FirstOrDefault();
            if (campaign == null)
                return null;

            campaignDetails.CampaignName = campaign.Name;

            switch (campaignDetails.Selection)
            {
                case ChartSelection.VotesDetails:
                    campaignDetails.PieChart = mapper.Map<Company, PieChart>(campaign);
                    campaignDetails.StackedChart = null;
                    break;
                case ChartSelection.VotesDetailsByTime:
                    try { campaignDetails.StackedChart = mapper.Map<Company, StackedChart>(campaign); }
                    catch (AutoMapperMappingException) { campaignDetails.StackedChart = null; }
                    campaignDetails.PieChart = null;
                    break;
                default:
                    return null;
            }
            return campaignDetails;
        }

        public PieChart GetVotesChart(int campaignId, string userId)
        {
            var campaign = unitOfWork.Charts.Get(pcc => pcc.Id == campaignId
                && pcc.ApplicationGroup.ApplicationUsers.Any(au => au.Id == userId)).FirstOrDefault();

            return mapper.Map<Company, PieChart>(campaign);
        }

        public StackedChart GetVotesChartByTime(int campaignId, string userId)
        {
            var campaign = unitOfWork.Charts.Get(pcc => pcc.Id == campaignId
                && pcc.ApplicationGroup.ApplicationUsers.Any(au => au.Id == userId)).FirstOrDefault();

            try
            {
                return mapper.Map<Company, StackedChart>(campaign);
            }
            catch(AutoMapperMappingException)
            {
                return null;
            }
        }
    }
}
