using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.CampaignReportingViewModels
{
    public class CampaignDetailsViewModel
    {
        public ChartSelection Selection { get; set; }
        public bool HaveVoting { get; set; }
        public string CampaignName { get; set; }
        public int CampaignId { get; set; }
        
        public PieChart PieChart { get; set; }
        public StackedChart StackedChart { get; set; }
    }

    public enum ChartSelection
    {
        MailingDetails,
        VotesDetails,
        VotesDetailsByTime
    }
}
