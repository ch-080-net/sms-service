using WebCustomerApp.Models;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Data;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private IBaseRepository<Recipient> recipientRepo;
        private IBaseRepository<StopWord> stopWordRepo;
        private ICompanyRepository companyRepo;
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
        private IChartsRepository ChartsRepo;

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
                if (ChartsRepo == null)
                {
                    ChartsRepo = new ChartsRepository(context);
                }
                return ChartsRepo;
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
