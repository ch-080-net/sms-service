using AutoMapper;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public class StopWordManager: BaseManager,IStopWordManager
    {
        public StopWordManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork , mapper)
        {
        }

        public IEnumerable<StopWord> GetStopWords()
        {
            return unitOfWork.StopWords.GetAll();
        }
    }
}
