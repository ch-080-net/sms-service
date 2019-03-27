using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using Model.ViewModels.EmailCampaignViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    public class EmailCampaignManager : BaseManager, IEmailCampaignManager
    {
        public EmailCampaignManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

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
            unitOfWork.EmailCampaigns.Insert(emailCampaign);
            unitOfWork.Save();
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
    }
}
