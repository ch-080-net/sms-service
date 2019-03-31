using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public abstract class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationOrigin Origin { get; set; }
    }

    public enum NotificationOrigin
    {
        CampaignReport,
        PersonalNotification
    }
}
