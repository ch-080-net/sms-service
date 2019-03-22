using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.CampaingChartReportingViewModels
{
    public class PieChartCampaing
    {
        public string Description { get; set; }
        public IEnumerable<Tuple<string, int>> Categories { get; set; }
    }
}
