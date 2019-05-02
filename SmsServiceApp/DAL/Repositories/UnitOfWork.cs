using WebApp.Models;
using Model.Interfaces;
using System;
using WebApp.Data;


namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private IBaseRepository<Recipient> recipientRepo;
        private IBaseRepository<StopWord> stopWordRepo;
        private ICompanyRepository companyRepo;
        private IAdminStatisticRepository adminStatisticRepository;
        private IOperatorRepository operatorRepo;
        private IContactRepository contactRepo;
        private IBaseRepository<Phone> phoneRepo;
        private IBaseRepository<Tariff> tariffRepo;
        private IBaseRepository<Code> codeRepo;
        private IBaseRepository<ApplicationGroup> groupRepo;
        private IBaseRepository<PhoneGroupUnsubscribe> phoneGroupUnsubscribe;
        private IBaseRepository<RecievedMessage> recievedMessagesRepo;
        private IBaseRepository<AnswersCode> answersCodeRepo;
        private IMailingRepository mailingRepo;
        private IChartsRepository chartsRepo;
        private INotificationRepository notificationRepo;
        private ICampaignNotificationRepository campaignNotificationRepo;
        private IBaseRepository<ApplicationUser> appUserRepo;
        private IBaseRepository<SubscribeWord> subscribeWordRepo;
        private IBaseRepository<EmailRecipient> emailRecipientRepo;
        private IBaseRepository<CompanySubscribeWord> companySubscribeWordRepo;
        private IEmailCampaignRepository emailCampaignRepo;
        private IBaseRepository<Email> emailRepo;
        private IEmailCampaignNotificationRepository emailCampNotRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ICompanyRepository Companies
        {
            get
            {
                if (companyRepo == null) { companyRepo = new CompanyRepository(context); }
                return companyRepo;
            }
        }

        public IBaseRepository<Recipient> Recipients {
            get {
                if (recipientRepo == null) { recipientRepo = new BaseRepository<Recipient>(context); }
                return recipientRepo;
            }
        }


        public IBaseRepository<Tariff> Tariffs
        {
            get
            {
                if (tariffRepo == null) { tariffRepo = new BaseRepository<Tariff>(context); }
                return tariffRepo;
            }
        }
      

        public IBaseRepository<StopWord> StopWords
        {
            get
            {
                if (stopWordRepo == null) { stopWordRepo = new BaseRepository<StopWord>(context); }
                return stopWordRepo;
            }
        }

        public IBaseRepository<CompanySubscribeWord> CompanySubscribeWords
        {
            get
            {
                if (companySubscribeWordRepo == null) {companySubscribeWordRepo = new BaseRepository<CompanySubscribeWord>(context); }
                return companySubscribeWordRepo;
            }
        }

        public IOperatorRepository Operators
        {
            get
            {
                if (operatorRepo == null)
                {
                    operatorRepo = new OperatorRepository(context);
                }
                return operatorRepo;
            }
        }

        public IContactRepository Contacts {
            get {
                if (contactRepo == null) { contactRepo = new ContactRepository(context); }
                return contactRepo;
            }
        }

        public IBaseRepository<Phone> Phones {
            get {
                if (phoneRepo == null) { phoneRepo = new BaseRepository<Phone>(context); }
                return phoneRepo;
            }
        }

        public IBaseRepository<ApplicationGroup> ApplicationGroups
        {
            get
            {
                if (groupRepo == null) { groupRepo = new BaseRepository<ApplicationGroup>(context); }
                return groupRepo;
            }
        }

        public IBaseRepository<RecievedMessage> RecievedMessages
        {
            get
            {
                if (recievedMessagesRepo == null) { recievedMessagesRepo = new BaseRepository<RecievedMessage>(context); }
                return recievedMessagesRepo;
            }
        }

        public IBaseRepository<AnswersCode> AnswersCodes
        {
            get
            {
                if (answersCodeRepo == null) { answersCodeRepo = new BaseRepository<AnswersCode>(context); }
                return answersCodeRepo;
            }
        }

        public IBaseRepository<Code> Codes
        {
            get
            {
                if (codeRepo == null)
                {
                    codeRepo = new BaseRepository<Code>(context);
                }
                return codeRepo;
            }
        }

        public IBaseRepository<PhoneGroupUnsubscribe> PhoneGroupUnsubscribes
        {
            get
            {
                if (phoneGroupUnsubscribe == null)
                {
                    phoneGroupUnsubscribe = new BaseRepository<PhoneGroupUnsubscribe>(context);
                }
                return phoneGroupUnsubscribe;
            }
        }
        public IBaseRepository<SubscribeWord> SubscribeWords
        {
            get
            {
                if (subscribeWordRepo == null)
                {
                    subscribeWordRepo = new BaseRepository<SubscribeWord>(context);
                }
                return subscribeWordRepo;
            }
        }
        public IMailingRepository Mailings
        {
            get
            {
                if (mailingRepo == null)
                {
                    mailingRepo = new MailingRepository(context);
                }
                return mailingRepo;
            }
        }

        public IChartsRepository Charts
        {
            get
            {
                if (chartsRepo == null)
                {
                    chartsRepo = new ChartsRepository(context);
                }
                return chartsRepo;
            }
        }
        
        public IAdminStatisticRepository adminStatistic
        {
            get
            {
                if (adminStatisticRepository == null)
                {
                    adminStatisticRepository = new AdminStatisticRepository(context);
                }
                return adminStatisticRepository;
            }
        }
        public INotificationRepository Notifications
        {
            get
            {
                if (notificationRepo == null)
                {
                    notificationRepo = new NotificationRepository(context);
                }
                return notificationRepo;
            }
        }

        public ICampaignNotificationRepository CampaignNotifications
        {
            get
            {
                if (campaignNotificationRepo == null)
                {
                    campaignNotificationRepo = new CampaignNotificationRepository(context);
                }
                return campaignNotificationRepo;
            }
        }

        public IBaseRepository<ApplicationUser> ApplicationUsers
        {
            get
            {
                if (appUserRepo == null)
                {
                    appUserRepo = new BaseRepository<ApplicationUser>(context);
                }
                return appUserRepo;
            }
        }

        public IBaseRepository<EmailRecipient> EmailRecipients
        {
            get
            {
                if (emailRecipientRepo == null)
                {
                    emailRecipientRepo = new BaseRepository<EmailRecipient>(context);
                }
                return emailRecipientRepo;
            }
        }

        public IEmailCampaignRepository EmailCampaigns
        {
            get
            {
                if ( emailCampaignRepo == null)
                {
                    emailCampaignRepo = new EmailCampaignRepository(context);
                }
                return emailCampaignRepo;
            }
        }

        public IBaseRepository<Email> Emails
        {
            get
            {
                if (emailRepo == null)
                {
                    emailRepo = new BaseRepository<Email>(context);
                }
                return emailRepo;
            }
        }

        IEmailCampaignNotificationRepository IUnitOfWork.EmailCampaignNotifications
        {
            get
            {
                if (emailCampNotRepo == null)
                {
                    emailCampNotRepo = new EmailCampaignNotificactionRepository(context);
                }
                return emailCampNotRepo;
            }
        }


        public IAdminStatisticRepository AdminStatistics
        {
            get
            {
                if (adminStatisticRepository == null)
                {
                    adminStatisticRepository = new AdminStatisticRepository(context);
                }
                return adminStatisticRepository;
            }
        }
        public int Save()
        {
            return context.SaveChanges();
        }

        private bool isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                context.Dispose();
            }
            isDisposed = true;
        }
    }
}
