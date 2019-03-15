using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;
using Model.ViewModels.ChartsViewModels;

namespace Model.Interfaces
{
    public interface IChartsManager
    {
        PieChart GetVotesChart(int campaignId, string userId);

        StackedChart GetVotesChartByTime(int campaignId, string userId);
    }
}
