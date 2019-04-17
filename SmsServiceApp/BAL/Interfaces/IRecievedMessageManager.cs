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
        bool Insert(RecievedMessageDTO item);
        bool Delete(int id);
        bool SearchSubscribeWordInMessages(RecievedMessageDTO message);
        bool SearchStopWordInMessages(RecievedMessageDTO message);
    }
}
