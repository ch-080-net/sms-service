using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Model.ViewModels.CampaignDetailsViewModels
{
    public class PieChart
    {
        public string Description { get; set; }
        public IEnumerable<Tuple<string, int>> Categories { get; set; }
    }
}
