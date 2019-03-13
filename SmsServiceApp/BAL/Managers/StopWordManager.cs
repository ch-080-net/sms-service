using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.StopWordViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public class StopWordManager: BaseManager,IStopWordManager
    {
        public StopWordManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork , mapper)
        {
        }


        public IEnumerable<StopWordViewModel> GetStopWords()
        {
            IEnumerable<StopWord> words = unitOfWork.StopWords.GetAll();
            return mapper.Map<IEnumerable<StopWord>, IEnumerable<StopWordViewModel>>(words);
        }

        public void Insert(StopWordViewModel item)
        {
            StopWord word = mapper.Map<StopWordViewModel, StopWord>(item);
            unitOfWork.StopWords.Insert(word);
            unitOfWork.Save();
        }

        public void Update(StopWordViewModel item)
        {
            StopWord word = mapper.Map<StopWordViewModel, StopWord>(item);
            unitOfWork.StopWords.Update(word);
            unitOfWork.Save();
        }

        public void Delete(int item)
        {
            StopWord word = unitOfWork.StopWords.GetById(item);
            unitOfWork.StopWords.Delete(word);
            unitOfWork.Save();
        }


        public void SetStateModified(StopWordViewModel item)
        {
            StopWord word = mapper.Map<StopWordViewModel, StopWord>(item);
            unitOfWork.StopWords.SetStateModified(word);
            unitOfWork.Save();
        }

        //clear
        //public void SearchStopWordInMeaasge(string Originator ,string Destination ,string Content)
        //{
        //    Content = Content.Substring(Content.IndexOf(" Text: ") + 7);//7=" Text: " size

        //   var words = unitOfWork.StopWords.GetAll().FirstOrDefault(c => c.Word==Content);
        //    if (words != null)
        //    {
        //        var recipient = unitOfWork.Recipients.GetAll().FirstOrDefault(r => (r.Phone.PhoneNumber == Originator) && (r.Company.Phone.PhoneNumber == Destination));
        //        if (recipient != null)
        //        {
        //            recipient.IsStopped = true;
        //            unitOfWork.Recipients.Update(recipient);
        //        }
        //    }
        //}
    }
}
