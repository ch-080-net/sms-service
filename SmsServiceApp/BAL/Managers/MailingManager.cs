using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using AutoMapper;
using Model.DTOs;
using System.Threading.Tasks;
using System.Linq;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Mailing job
    /// </summary>
    public class MailingManager : BaseManager, IMailingManager
    {
        public MailingManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        /// <summary>
        /// Get messages wich was not been sent
        /// </summary>
        public IEnumerable<MessageDTO> GetUnsentMessages()
        {
            var recipients = unitOfWork.Mailings.Get(r => r.MessageState == MessageState.NotSent
                                                        && r.Company.SendingTime >= DateTime.UtcNow
                                                        && !r.Company.ApplicationGroup.phoneGroupUnsubscribtions.Any(pgu => pgu.PhoneId == r.PhoneId));
            IEnumerable<MessageDTO> result = mapper.Map<IEnumerable<Recipient>, IEnumerable<MessageDTO>>(recipients);
            return result;
        }


        public void MarkAs(IEnumerable<MessageDTO> messages, MessageState messageState)
        {
            var recipientIds = from m in messages
                               select m.RecipientId;

            foreach (var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                if (tempRecipient != null)
                    tempRecipient.MessageState = messageState;
            }
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAs(MessageDTO messages, MessageState messageState)
        {
            var tempRecipient = unitOfWork.Mailings.GetById(messages.RecipientId);
            if (tempRecipient != null)
                tempRecipient.MessageState = messageState;
            try { unitOfWork.Save(); }
            finally { }
        }
    }
}
