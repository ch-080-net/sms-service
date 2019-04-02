using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.CampaignReportingViewModels
{
   public class CompaingPieChart
    {
        public string Description { get; set; }
        public IEnumerable<Tuple<string, int>> Categories { get; set; }
    }
}
