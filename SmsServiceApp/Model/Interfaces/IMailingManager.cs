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

        void MarkAsSent(IEnumerable<MessageDTO> messages);
        void MarkAsSent(MessageDTO messages);
    }
}
