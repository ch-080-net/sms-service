using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public class StopWordManager: BaseManager,IStopWordManager
    {
        public StopWordManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<StopWord> GetStopWords()
        {
            return unitOfWork.StopWords.GetAll();
        }
    }
}
