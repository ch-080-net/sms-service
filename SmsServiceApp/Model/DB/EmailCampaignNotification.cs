using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class EmailCampaignNotification
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public EmailCampaign EmailCampaign { get; set; }
        public NotificationType Type { get; set; }
        public bool BeenSent { get; set; }

    }
}
