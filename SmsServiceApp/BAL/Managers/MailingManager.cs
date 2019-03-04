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
        public async Task<IEnumerable<MessageDTO>> GetUnsentMessages()
        {
            var recipients = unitOfWork.Mailings.Get(r => !r.BeenSent);
            IEnumerable<MessageDTO> result = mapper.Map<IEnumerable<Recipient>, IEnumerable<MessageDTO>>(recipients);
            return result;
        }

        public async Task MarkAsSent(IEnumerable<MessageDTO> messages)
        {
            var recipientIds = from m in messages
            select m.RecipientId;

            foreach(var id in recipientIds)
            {
                var tempRecipient = unitOfWork.Mailings.GetById(id);
                unitOfWork.Mailings.Delete(tempRecipient);
            }
            unitOfWork.Save();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    base.unitOfWork.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
