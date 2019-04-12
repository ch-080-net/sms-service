using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class SmsNotificationDTO : NotificationDTO
    {
        public string SenderPhone { get; set; }
        public string RecieverPhone { get; set; }
        public string Message { get; set; }
    }
}
