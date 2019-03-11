using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using AutoMapper;
using Model.DTOs;
using System.Threading.Tasks;
using System.Linq;
using WebCustomerApp.Models;

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
            var recipients = unitOfWork.Mailings.Get(r => r.MessageState == 0);
            IEnumerable<MessageDTO> result = mapper.Map<IEnumerable<Recipient>, IEnumerable<MessageDTO>>(recipients);
            return result;
        }

        /// <summary>
        /// Find recipients of messages and mark them as sent
        /// </summary>
        /// <param name="messages">Should contain RecipientId</param>
        public void MarkAsSent(IEnumerable<MessageDTO> messages)
        {
            var recipientIds = from m in messages
            select m.RecipientId;

            foreach(var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                if (tempRecipient != null)
                    tempRecipient.MessageState = 0;
            }
            try
            {
                unitOfWork.Save();
            }
            catch
            {

            }
        }


        public void MarkAsSent(MessageDTO messages)
        {
            var tempRecipient = unitOfWork.Mailings.GetById(messages.RecipientId);
            if (tempRecipient != null)
                tempRecipient.MessageState = 0;
            try
            {
                unitOfWork.Save();
            }
            catch
            {

            }
        }
    }
}
