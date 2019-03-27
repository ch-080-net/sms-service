using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.CampaignReportingViewModels
{
    public class CampaignDetailsViewModel
    {
        public ChartSelection Selection { get; set; }
        public bool HaveVoting { get; set; }
        public string CampaignName { get; set; }

        [Required]
        public int CampaignId { get; set; }
        public CompaingPieChart CompaingPieChart { get; set; }
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
