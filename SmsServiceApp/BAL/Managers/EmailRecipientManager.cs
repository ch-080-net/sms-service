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
            try
            {
                EmailRecipient emailRecipient = unitOfWork.EmailRecipients.GetById(id);
                emailRecipient.Email = unitOfWork.Emails.GetById(emailRecipient.EmailId);
                return mapper.Map<EmailRecipientViewModel>(emailRecipient);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Insert(EmailRecipientViewModel item, int companyId)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(EmailRecipientViewModel item)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }           
        }

        public void Delete(int id)
        {
            try
            {
                EmailRecipient emailRecipient = unitOfWork.EmailRecipients.GetById(id);
                unitOfWork.EmailRecipients.Delete(emailRecipient);
                unitOfWork.Save();
            }
            catch (Exception e)
            {
                throw e;
            }           
        }

    }
}
