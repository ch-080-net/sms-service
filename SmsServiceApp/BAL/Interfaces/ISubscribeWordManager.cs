using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.SubscribeWordViewModels;

namespace BAL.Interfaces
{
   public interface ISubscribeWordManager
    {
        IEnumerable<SubscribeWordViewModel> GetStopWords();
        void Insert(SubscribeWordViewModel item);
        void Update(SubscribeWordViewModel item);
       
    }
}
