using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using Model.ViewModels.EmailRecipientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    public class EmailRecipientManager : BaseManager, IEmailRecipientManager
    {
        public EmailRecipientManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public List<EmailRecipientViewModel> GetEmailRecipients(int companyId)
        {
            IEnumerable<EmailRecipient> emailRecipients = unitOfWork.EmailRecipients.Get(filter: er => er.CompanyId == companyId);
            foreach (var er in emailRecipients)
            {
                er.Email = unitOfWork.Emails.GetById(er.EmailId);
            }
            return mapper.Map<IEnumerable<EmailRecipient>, List<EmailRecipientViewModel>>(emailRecipients);
        }

        public EmailRecipientViewModel GetEmailRecipientById(int id)
        {
            EmailRecipient emailRecipient = unitOfWork.EmailRecipients.GetById(id);
            emailRecipient.Email = unitOfWork.Emails.GetById(emailRecipient.EmailId);
            return mapper.Map<EmailRecipientViewModel>(emailRecipient);
        }

        public void Insert(EmailRecipientViewModel item, int companyId)
        {
            EmailRecipient emailRecipient = mapper.Map<EmailRecipient>(item);
            emailRecipient.CompanyId = companyId;
            Email email = unitOfWork.Emails.Get(filter: e => e.EmailAddress == item.EmailAddress).FirstOrDefault();
            if (email == null)
            {
                email = new Email();
                email.EmailAddress = item.EmailAddress;
                unitOfWork.Emails.Insert(email);
                emailRecipient.Email = email;
            }
            else
            {
                emailRecipient.EmailId = email.Id;
            }
            unitOfWork.EmailRecipients.Insert(emailRecipient);
            unitOfWork.Save();
        }

        public void Update(EmailRecipientViewModel item)
        {
            EmailRecipient emailRecipient = mapper.Map<EmailRecipient>(item);
            Email email = unitOfWork.Emails.Get(filter: e => e.EmailAddress == item.EmailAddress).FirstOrDefault();
            if (email == null)
            {
                email = new Email();
                email.EmailAddress = item.EmailAddress;
                unitOfWork.Emails.Insert(email);
                emailRecipient.Email = email;
            }
            else
            {
                emailRecipient.EmailId = email.Id;
            }
            unitOfWork.EmailRecipients.Update(emailRecipient);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            EmailRecipient emailRecipient = unitOfWork.EmailRecipients.GetById(id);
            unitOfWork.EmailRecipients.Delete(emailRecipient);
            unitOfWork.Save();
        }

    }
}
