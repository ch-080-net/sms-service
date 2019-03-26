using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public NotificationType Type { get; set; }
        public DateTime Time { get; set; }
        public bool BeenSent { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public enum NotificationType
    {
        Site,
        Email,
        SMS
    }
}
