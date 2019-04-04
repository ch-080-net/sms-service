using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class EmailRecipient
    {
        public int Id { get; set; }

        public int EmailId { get; set; }
        public Email Email { get; set; }
        public int? CompanyId { get; set; }
        public EmailCampaign Company { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public string Priority { get; set; }
        public string KeyWords { get; set; }
        public byte IsSend { get; set; }
    }
}
