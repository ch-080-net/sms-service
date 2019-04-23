using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Model.DTOs
{
    public class NotificationReportDTO
    {
        public IEnumerable<WebNotificationDTO> Notifications { get; set; }
        public int VotingsInProgress { get; set; }
        public int MailingsPlannedToday { get; set; }

        public static NotificationReportDTO operator + (NotificationReportDTO nr1, NotificationReportDTO nr2)
        {
            var result = new NotificationReportDTO();
            result.MailingsPlannedToday = nr1.MailingsPlannedToday + nr2.MailingsPlannedToday;
            result.VotingsInProgress = nr1.VotingsInProgress + nr2.VotingsInProgress;
            result.Notifications = (nr1.Notifications ?? new List<WebNotificationDTO>())
                .Concat(nr2.Notifications ?? new List<WebNotificationDTO>());
            return result;
        }
    }
}
