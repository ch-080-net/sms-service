using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.CampaignReportingViewModels
{
    public class StackedChart
    {
        public string Description { get; set; }
        public IEnumerable<string> TimeFrame { get; set; }
        public IEnumerable<Tuple<string, IEnumerable<int>>> Categories { get; set; }
    }
}
