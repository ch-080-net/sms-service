using AutoMapper;
using BAL.Interfaces;
using Model.DTOs;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    public class EmailMailingManager : BaseManager, IEmailMailingManager
    {
        public EmailMailingManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public IEnumerable<EmailDTO> GetUnsentEmails()
        {
            var recepients = unitOfWork.EmailRecipients.Get(filter: r => r.IsSend == 0);
            foreach (var rec in recepients)
            {
                rec.Email = unitOfWork.Emails.GetById(rec.EmailId);
                rec.Company = unitOfWork.EmailCampaigns.GetById((int)rec.CompanyId);
                rec.Company.Email = unitOfWork.Emails.GetById((int)rec.Company.EmailId);
            }
            recepients = recepients.Where(r => r.Company.SendingTime <= DateTime.Now);
            return mapper.Map<IEnumerable<EmailRecipient>, IEnumerable<EmailDTO>>(recepients);
        }

        public void MarkAs(EmailDTO messages, byte messageState)
        {
            var tempRecipient = unitOfWork.EmailRecipients.GetById(messages.EmailRecipientId);
            if (tempRecipient != null)
                tempRecipient.IsSend = messageState;
            unitOfWork.Save();
        }
    }
}
