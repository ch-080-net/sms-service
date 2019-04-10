using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Model.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Recipient> Recipients { get; }
        IContactRepository Contacts { get; }
        IBaseRepository<Phone> Phones { get; }
        ICompanyRepository Companies { get; }
        IOperatorRepository Operators { get; }
        IBaseRepository<Code> Codes { get; }
        IBaseRepository<Tariff> Tariffs { get; }
        IBaseRepository<StopWord> StopWords { get; }
        IBaseRepository<PhoneGroupUnsubscribe> PhoneGroupUnsubscribes { get; }
        IMailingRepository Mailings { get; }
        IChartsRepository Charts { get; }
        INotificationRepository Notifications { get; }
        ICampaignNotificationRepository CampaignNotifications { get; }
        IEmailCampaignNotificationRepository EmailCampaignNotifications { get; }
        IBaseRepository<ApplicationUser> ApplicationUsers { get; }
        IBaseRepository<SubscribeWord> SubscribeWords { get; }

        IBaseRepository<ApplicationGroup> ApplicationGroups { get; }
        IBaseRepository<RecievedMessage> RecievedMessages { get; }
        IBaseRepository<AnswersCode> AnswersCodes { get; }
        IBaseRepository<EmailRecipient> EmailRecipients { get; }
        IEmailCampaignRepository EmailCampaigns { get; }
        IBaseRepository<Email> Emails { get; }
        IAdminStatisticRepository AdminStatistics { get; }



        int Save();
    }

}
