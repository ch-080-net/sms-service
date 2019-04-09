using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class NotificationReportDTO
    {
        public IEnumerable<WebNotificationDTO> Notifications { get; set; }
        public int VotingsInProgress { get; set; }
        public int MailingsPlannedToday { get; set; }
    }
}
