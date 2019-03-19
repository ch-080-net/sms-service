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
        /// <summary>
        /// Manager for StopWord, include all methods needed to work with StopWord storage.
        /// Inherited from BaseManager and have additional methods.
        /// </summary>
        public StopWordManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork , mapper)
        {
        }

        /// <summary>
        /// Method for getting all stop words from db
        /// </summary>
        /// <returns>IEnumerable of mapped to ViewModel objects</returns>
        public IEnumerable<StopWordViewModel> GetStopWords()
        {
            IEnumerable<StopWord> words = unitOfWork.StopWords.GetAll();
            return mapper.Map<IEnumerable<StopWord>, IEnumerable<StopWordViewModel>>(words);
        }
        /// <summary>
        /// Method for inserting new stopwod to db
        /// </summary>
        /// <param name="item">ViewModel of stopword</param>
        public void Insert(StopWordViewModel item)
        {
            StopWord word = mapper.Map<StopWordViewModel, StopWord>(item);
            unitOfWork.StopWords.Insert(word);
            unitOfWork.Save();
        }
        /// <summary>
        ///  Update stop word in db
        /// </summary>
        /// <param name="item">ViewModel of stopword</param>
        public void Update(StopWordViewModel item)
        {
            StopWord word = mapper.Map<StopWordViewModel, StopWord>(item);
            unitOfWork.StopWords.Update(word);
            unitOfWork.Save();
        }
        /// <summary>
        /// delete stop word by db
        /// </summary>
        /// <param id="item"></param>
        public void Delete(int item)
        {
            StopWord word = unitOfWork.StopWords.GetById(item);
            unitOfWork.StopWords.Delete(word);
            unitOfWork.Save();
        }


    }
}
