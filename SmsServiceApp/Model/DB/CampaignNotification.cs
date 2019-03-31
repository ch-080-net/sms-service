using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class CampaignNotification
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Company Campaign { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public CampaignNotificationType Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool BeenSent { get; set; }

    }

    public enum CampaignNotificationType
    {
        Web,
        Sms,
        Email
    }
}
