using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IMailingManager
    {
        IEnumerable<MessageDTO> GetUnsentMessages();

        void MarkAsDelivered(IEnumerable<MessageDTO> messages);
        void MarkAsDelivered(MessageDTO messages);
        void MarkAsAccepted(IEnumerable<MessageDTO> messages);
        void MarkAsAccepted(MessageDTO messages);
        void MarkAsUndeliverable(IEnumerable<MessageDTO> messages);
        void MarkAsUndeliverable(MessageDTO messages);
        void MarkAsRejected(IEnumerable<MessageDTO> messages);
        void MarkAsRejected(MessageDTO messages);
    }
}
