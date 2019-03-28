using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class WebNotificationDTO : NotificationDTO
    {
        public string UserId { get; set; }
        public DateTime Time { get; set; }
    }
}
