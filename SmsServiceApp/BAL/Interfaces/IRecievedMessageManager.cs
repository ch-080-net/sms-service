using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IRecievedMessageManager
    {
        RecievedMessageDTO Get(int id);
        IEnumerable<RecievedMessageDTO> GetRecievedMessages(int companyId);
        void Insert(RecievedMessageDTO item);
        void Delete(int id);
    }
}
