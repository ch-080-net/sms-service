using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Model.DTOs
{
    public class NotificationDTO
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public int Id { get; set; }        
        public NotificationOrigin Origin { get; set; }
    }

    public enum NotificationOrigin
    {
        CampaignReport,
        PersonalNotification,
        EmailCampaignReport
    }
}
