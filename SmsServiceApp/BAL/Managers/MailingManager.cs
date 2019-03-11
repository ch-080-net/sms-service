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
            var recipients = unitOfWork.Mailings.Get(r => r.MessageState == MessageState.NotSent);
            IEnumerable<MessageDTO> result = mapper.Map<IEnumerable<Recipient>, IEnumerable<MessageDTO>>(recipients);
            return result;
        }

        public void MarkAsAccepted(IEnumerable<MessageDTO> messages)
        {
            var recipientIds = from m in messages
                               select m.RecipientId;

            foreach (var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                if (tempRecipient != null)
                    tempRecipient.MessageState = MessageState.Accepted;
            }
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsAccepted(MessageDTO messages)
        {
            var tempRecipient = unitOfWork.Mailings.GetById(messages.RecipientId);
            if (tempRecipient != null)
                tempRecipient.MessageState = MessageState.Accepted;
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsDelivered(IEnumerable<MessageDTO> messages)
        {
            var recipientIds = from m in messages
                               select m.RecipientId;

            foreach (var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                if (tempRecipient != null)
                    tempRecipient.MessageState = MessageState.Delivered;
            }
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsDelivered(MessageDTO messages)
        {
            var tempRecipient = unitOfWork.Mailings.GetById(messages.RecipientId);
            if (tempRecipient != null)
                tempRecipient.MessageState = MessageState.Delivered;
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsRejected(IEnumerable<MessageDTO> messages)
        {
            var recipientIds = from m in messages
                               select m.RecipientId;

            foreach (var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                if (tempRecipient != null)
                    tempRecipient.MessageState = MessageState.Rejected;
            }
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsRejected(MessageDTO messages)
        {
            var tempRecipient = unitOfWork.Mailings.GetById(messages.RecipientId);
            if (tempRecipient != null)
                tempRecipient.MessageState = MessageState.Rejected;
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsUndeliverable(IEnumerable<MessageDTO> messages)
        {
            var recipientIds = from m in messages
                               select m.RecipientId;

            foreach (var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                if (tempRecipient != null)
                    tempRecipient.MessageState = MessageState.Undeliverable;
            }
            try { unitOfWork.Save(); }
            finally { }
        }

        public void MarkAsUndeliverable(MessageDTO messages)
        {
            var tempRecipient = unitOfWork.Mailings.GetById(messages.RecipientId);
            if (tempRecipient != null)
                tempRecipient.MessageState = MessageState.Undeliverable;
            try { unitOfWork.Save(); }
            finally { }
        }
    }
}
