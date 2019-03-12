using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;

namespace Model.Interfaces
{
    public interface IPoolCampaignChartsManager
    {
        PieChartDTO GetPieChart(int campaignId, string userId);
    }
}
