using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class EmailCampaign
    {
        public int Id { get; set; }
        public int? EmailId { get; set; } 
        public Email Email { get; set; }
        public ICollection<EmailRecipient> EmailRecipients { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Message { get; set; }
        public DateTime SendingTime { get; set; }
    }
}
