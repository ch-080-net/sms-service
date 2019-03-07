using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IMailingManager
    {
        Task<IEnumerable<MessageDTO>> GetUnsentMessages();

        Task MarkAsSent(IEnumerable<MessageDTO> messages);
        Task MarkAsSent(MessageDTO messages);
    }
}
