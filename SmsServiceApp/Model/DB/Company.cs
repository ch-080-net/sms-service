using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    /// <summary>
    /// Company entity, which iclude message, recipients and chosen tariff
    /// </summary>
    public class Company
    {
        public int Id { get; set; } 
        public ICollection<Recipient> Recipients { get; set; }
        public ICollection<RecievedMessage> RecievedMessages { get; set; }
        public ICollection<AnswersCode> AnswersCodes { get; set; }
        public ICollection<CampaignNotification> CampaignNotifications { get; set; }
        public ICollection<SubscribeWord> SubscribeWords { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ApplicationGroupId { get; set; } 
        public ApplicationGroup ApplicationGroup { get; set; }
        public int? PhoneId { get; set; }
        public Phone Phone { get; set; }
        public int? TariffId { get; set; }
        public Tariff Tariff { get; set; }
        public string Message { get; set; }
        public CompanyType Type { get; set; }
        public bool IsPaused { get; set; } 
        public DateTime SendingTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
