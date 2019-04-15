using Model.DTOs;
using Model.ViewModels.RecievedMessageViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IRecievedMessageManager
    {
        RecievedMessageViewModel Get(int id);
        IEnumerable<RecievedMessageViewModel> GetRecievedMessages(int companyId);
        void Insert(RecievedMessageDTO item);
        void Delete(int id);
        void SearchSubscribeWordInMessages(RecievedMessageDTO message);
        void SearchStopWordInMessages(RecievedMessageDTO message);
    }
}
