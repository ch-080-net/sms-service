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
            foreach (var word in words)
            {
                word.Phone = unitOfWork.Phones.GetById((int)word.SubscribePhoneId);
            }
            return mapper.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(words);
        }

        public IEnumerable<SubscribeWordViewModel> GetWordsByCompanyId(int companyId)
        {
            IEnumerable<SubscribeWord> words = unitOfWork.SubscribeWords.GetAll().Where(sw=>sw.CompanyId==companyId);
            foreach (var word in words)
            {
                word.Phone = unitOfWork.Phones.GetById((int)word.SubscribePhoneId);
            }
            return mapper.Map<IEnumerable<SubscribeWord>, IEnumerable<SubscribeWordViewModel>>(words);
        }
        /// <summary>
        /// Method for inserting new stopwod to db
        /// </summary>
        /// <param name="item">ViewModel of stopword</param>
        public void Insert(SubscribeWordViewModel item)
        {
            SubscribeWord subscribeWord = mapper.Map<SubscribeWordViewModel, SubscribeWord>(item);
            List<Phone> phone = unitOfWork.Phones.Get(p => p.PhoneNumber == item.PhoneNumber).ToList();

            if (phone.Count == 0)
            {
                Phone newPhone = new Phone();
                newPhone.PhoneNumber = item.PhoneNumber;
                unitOfWork.Phones.Insert(newPhone);
                unitOfWork.Save();
                subscribeWord.Phone = newPhone;
            }
            else
            {
                subscribeWord.Phone = phone[0];
            }
            unitOfWork.SubscribeWords.Insert(subscribeWord);
            unitOfWork.Save();
         
        }
        /// <summary>
        ///  Update Subscribe Word in db
        /// </summary>
        /// <param name="item">ViewModel of SubscribeWord</param>
        public void Update(SubscribeWordViewModel item)
        {
                SubscribeWord word = mapper.Map<SubscribeWordViewModel, SubscribeWord>(item);
                List<Phone> phone = unitOfWork.Phones.Get(p => p.PhoneNumber == item.PhoneNumber).ToList();

                if (phone.Count == 0)
                {
                    Phone newPhone = new Phone();
                    newPhone.PhoneNumber = item.PhoneNumber;
                    unitOfWork.Phones.Insert(newPhone);
                    unitOfWork.Save();
                    word.Phone = newPhone;
                }
                else
                {
                    word.Phone = phone[0];
                }
               
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
