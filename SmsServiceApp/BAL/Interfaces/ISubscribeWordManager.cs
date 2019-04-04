using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.SubscribeWordViewModels;

namespace BAL.Interfaces
{
   public interface ISubscribeWordManager
    {
        IEnumerable<SubscribeWordViewModel> GetWords();
        IEnumerable<SubscribeWordViewModel> GetWordsByCompanyId(int companyId);
        void Insert(SubscribeWordViewModel item);
        void Update(SubscribeWordViewModel item);
        void Delete(int item);

    }
}
