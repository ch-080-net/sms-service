using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class WebNotificationDTO : NotificationDTO
    {        
        public string Time { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Href { get; set; }

		public int CampaignId { get; set; }
    }
}
