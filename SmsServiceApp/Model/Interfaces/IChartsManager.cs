using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;
using Model.ViewModels.ChartsViewModels;

namespace Model.Interfaces
{
    public interface IChartsManager
    {
        PieChart GetPieChart(int campaignId, string userId);

        StackedChart GetStackedChart(int campaignId, string userId);
    }
}
