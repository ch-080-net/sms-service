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
        public IEnumerable<SubscribeWordViewModel> GetWordsByCompanyId(int companyId)
        {
            IEnumerable<SubscribeWord> words = unitOfWork.SubscribeWords.GetAll().Where(sw=>sw.CompanyId==companyId);
            return mapper.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(words);
        }
        /// <summary>
        /// Method for inserting new stopwod to db
        /// </summary>
        /// <param name="item">ViewModel of stopword</param>
        public void Insert(SubscribeWordViewModel item)
        {
            
                SubscribeWord word = mapper.Map<SubscribeWordViewModel, SubscribeWord>(item);
                unitOfWork.SubscribeWords.Insert(word);
                unitOfWork.Save();
         
        }
        /// <summary>
        ///  Update Subscribe Word in db
        /// </summary>
        /// <param name="item">ViewModel of SubscribeWord</param>
        public void Update(SubscribeWordViewModel item)
        {
                SubscribeWord word = mapper.Map<SubscribeWordViewModel, SubscribeWord>(item);
                unitOfWork.SubscribeWords.Update(word);
                unitOfWork.Save();
         
        }

        public void Delete(int item)
        {
                SubscribeWord word = unitOfWork.SubscribeWords.GetById(item);
                unitOfWork.SubscribeWords.Delete(word);
                unitOfWork.Save();
        }
    }
}
