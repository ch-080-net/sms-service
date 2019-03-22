using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.CampaingChartReportingViewModels
{
    public class CampaignChartViewModel
    {

        public string CampaignName { get; set; }
        [Required]
        public int CampaignId { get; set; }

        public PieChartCampaing PieChartCampaing { get; set; }
    }
}
