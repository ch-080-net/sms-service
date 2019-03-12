using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using AutoMapper;
using Model.DTOs;
using System.Threading.Tasks;
using System.Linq;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class PoolCampaignChartsManager : BaseManager, IPoolCampaignChartsManager
    {
        public PoolCampaignChartsManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public PieChartDTO GetPieChart(int campaignId, string userId)
        {
            var campaign = unitOfWork.PoolCampaignCharts.Get(pcc => pcc.Id == campaignId
                && pcc.ApplicationGroup.ApplicationUsers.Any(au => au.Id == userId)).FirstOrDefault();

            return mapper.Map<Company, PieChartDTO>(campaign);
        }
    }
}
