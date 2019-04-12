using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class EmailNotificationDTO : NotificationDTO
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
