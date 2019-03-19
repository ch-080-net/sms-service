using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using AutoMapper;
using Model.DTOs;
using System.Threading.Tasks;
using System.Linq;
using WebApp.Models;
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
            if (campaign.Type == CompanyType.Send)
            {
                campaignDetails.HaveVoting = false;
                campaignDetails.Selection = ChartSelection.MailingDetails;
            }
            else
                campaignDetails.HaveVoting = true;


            switch (campaignDetails.Selection)
            {
                case ChartSelection.VotesDetails:
                    campaignDetails.PieChart = mapper.Map<Company, PieChart>(campaign);
                    break;
                case ChartSelection.VotesDetailsByTime:
                    campaignDetails.StackedChart = mapper.Map<Company, StackedChart>(campaign);
                    break;
                default:
                    return campaignDetails;
            }
            return campaignDetails;
        }

    }
}
