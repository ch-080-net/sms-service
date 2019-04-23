using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using Model.ViewModels.SubscribeWordViewModels;
using WebApp.Models;

namespace BAL.Managers
{
    public class SubscribeWordManager: BaseManager,ISubscribeWordManager
    {
        /// <summary>
        /// Manager for SubscribeWord, include all methods needed to work with StopWord storage.
        /// Inherited from BaseManager and have additional methods.
        /// </summary>
        public SubscribeWordManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public IEnumerable<SubscribeWordViewModel> GetWords()
        {
            IEnumerable<SubscribeWord> words = unitOfWork.SubscribeWords.GetAll();
           
            return mapper.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(words);
        }
       
        /// <summary>
        /// Method for inserting new stopwod to db
        /// </summary>
        /// <param name="item">ViewModel of stopword</param>
        public void Insert(SubscribeWordViewModel item)
        {
            var word = unitOfWork.SubscribeWords.Get(w => w.Word == item.Word).FirstOrDefault();

            if (word!=null)
            {
                var compSw = unitOfWork.CompanySubscribeWords.Get(cw =>
                    cw.SubscribeWordId == word.Id && cw.CompanyId == item.CompanyId);
                if (!compSw.Any())
                {
                    unitOfWork.CompanySubscribeWords.Insert(new CompanySubscribeWord()
                        {SubscribeWordId = word.Id, CompanyId = item.CompanyId});
                }
            }
            else
            {
                word = mapper.Map<SubscribeWordViewModel, SubscribeWord>(item);
                unitOfWork.CompanySubscribeWords.Insert(new CompanySubscribeWord()
                    { SubscribeWord = word, CompanyId = item.CompanyId });
            }
          
            unitOfWork.Save();
        }
        /// <summary>
        ///  Update stop word in db
        /// </summary>
        /// <param name="item">ViewModel of stopword</param>
        public void Update(SubscribeWordViewModel item)
        {
            SubscribeWord word = mapper.Map<SubscribeWordViewModel, SubscribeWord>(item);
            unitOfWork.SubscribeWords.Update(word);
            unitOfWork.Save();
        }
        public IEnumerable<SubscribeWordViewModel> GetWordsByCompanyId(int companyId)
          {
              IEnumerable<CompanySubscribeWord> companySubscribes = unitOfWork.CompanySubscribeWords.GetAll().Where(cw => cw.CompanyId == companyId);

              List<SubscribeWord> words = new List<SubscribeWord>();

              foreach (var companySubscribe in companySubscribes)
              {
                words.Add(unitOfWork.SubscribeWords.Get(csw => csw.Id == companySubscribe.SubscribeWordId).FirstOrDefault());

              }
              return mapper.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(words);
          }
     
        public void Delete(int item)
        {
                SubscribeWord word = unitOfWork.SubscribeWords.GetById(item);
                unitOfWork.SubscribeWords.Delete(word);
                unitOfWork.Save();
        }
    }
}
