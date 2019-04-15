using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using Model.ViewModels.EmailCampaignViewModels;
using Model.ViewModels.EmailRecipientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;
using WebApp.Services;
using BAL.Notifications.Infrastructure;
using BAL.Notifications;

namespace BAL.Managers
{
    public class EmailCampaignManager : BaseManager, IEmailCampaignManager
    {
        private readonly INotificationsGenerator<EmailCampaign> notificationsGenerator;

        public EmailCampaignManager(IUnitOfWork unitOfWork, IMapper mapper
            , INotificationsGenerator<EmailCampaign> notificationsGenerator) : base(unitOfWork, mapper)
        {
            this.notificationsGenerator = notificationsGenerator;
        }

        public EmailCampaignViewModel GetById(int id)
        {
            EmailCampaign emailCampaign = unitOfWork.EmailCampaigns.GetById(id);
            emailCampaign.Email = unitOfWork.Emails.GetById((int)emailCampaign.EmailId);
            return mapper.Map<EmailCampaignViewModel>(emailCampaign);
        }

        public List<EmailCampaignViewModel> GetCampaigns(string userId, int page, int countOnPage, string searchValue)
        {
            IEnumerable<EmailCampaign> emailCampaigns = unitOfWork.EmailCampaigns.Get(ec => ec.UserId == userId
                && ec.Name.Contains(searchValue)).Skip((page - 1) * countOnPage).Take(countOnPage);
            foreach (var ec in emailCampaigns)
            {
                ec.Email = unitOfWork.Emails.GetById((int)ec.EmailId);
            }

            return mapper.Map<IEnumerable<EmailCampaign>, List<EmailCampaignViewModel>>(emailCampaigns);
        }

        public int GetCampaignsCount(string userId, string searchValue)
        {
            return unitOfWork.EmailCampaigns.Get(ec => ec.UserId == userId && ec.Name.Contains(searchValue)).Count();
        }

        public void Insert(EmailCampaignViewModel item)
        {
            EmailCampaign emailCampaign = mapper.Map<EmailCampaign>(item);
            Email email = unitOfWork.Emails.Get(filter: e => e.EmailAddress == item.EmailAddress).FirstOrDefault();
            if (email == null)
            {
                email = new Email();
                email.EmailAddress = item.EmailAddress;
                unitOfWork.Emails.Insert(email);
                emailCampaign.Email = email;
            }
            else
            {
                emailCampaign.EmailId = email.Id;
            }
            notificationsGenerator.SupplyWithCampaignNotifications(emailCampaign);
            unitOfWork.EmailCampaigns.Insert(emailCampaign);
            unitOfWork.Save();
        }

        public int InsertWithId(EmailCampaignViewModel item)
        {
            EmailCampaign company = mapper.Map<EmailCampaign>(item);
            notificationsGenerator.SupplyWithCampaignNotifications(company);
            int id = unitOfWork.EmailCampaigns.InsertWithId(company);
            return id;
        }

        public void Update(EmailCampaignViewModel item)
        {
            EmailCampaign emailCampaign = mapper.Map<EmailCampaign>(item);
            Email email = unitOfWork.Emails.Get(filter: e => e.EmailAddress == item.EmailAddress).FirstOrDefault();
            if (email == null)
            {
                email = new Email();
                email.EmailAddress = item.EmailAddress;
                unitOfWork.Emails.Insert(email);
                emailCampaign.Email = email;
            }
            else
            {
                emailCampaign.EmailId = email.Id;
            }
            unitOfWork.EmailCampaigns.Update(emailCampaign);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            EmailCampaign emailCampaign = unitOfWork.EmailCampaigns.GetById(id);
            unitOfWork.EmailCampaigns.Delete(emailCampaign);
            unitOfWork.Save();
        }

        public void IncertWithRecepients(EmailCampaignViewModel campaign, List<EmailRecipientViewModel> emailRecipients)
        {
            EmailCampaign emailCampaign = mapper.Map<EmailCampaign>(campaign);
            Email email = unitOfWork.Emails.Get(filter: e => e.EmailAddress == campaign.EmailAddress).FirstOrDefault();
            if (email == null)
            {
                email = new Email();
                email.EmailAddress = campaign.EmailAddress;
                unitOfWork.Emails.Insert(email);
                emailCampaign.Email = email;
            }
            else
            {
                emailCampaign.EmailId = email.Id;
            }
            notificationsGenerator.SupplyWithCampaignNotifications(emailCampaign);
            unitOfWork.EmailCampaigns.Insert(emailCampaign);
            unitOfWork.Save();
            foreach (var recipient in emailRecipients)
            {
                EmailRecipient newRecepient = mapper.Map<EmailRecipientViewModel, EmailRecipient>(recipient);
                newRecepient.CompanyId = emailCampaign.Id;
                email = unitOfWork.Emails.Get(filter: e => e.EmailAddress == recipient.EmailAddress).FirstOrDefault();
                if (email == null)
                {
                    email = new Email();
                    email.EmailAddress = recipient.EmailAddress;
                    unitOfWork.Emails.Insert(email);
                    newRecepient.Email = email;
                }
                else
                {
                    newRecepient.EmailId = email.Id;
                }
                unitOfWork.EmailRecipients.Insert(newRecepient);
                unitOfWork.Save();
            }
        }        
    }
}
