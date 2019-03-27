using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class Email
    {
        public int Id { get; set; }
        public ICollection<EmailRecipient> EmailRecipients { get; set; }
        public ICollection<EmailCampaign> EmailCampaigns { get; set; }

        public string EmailAddress { get; set; }
    }
}
