using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using Model.Interfaces;
using AutoMapper;
using Model.ViewModels.RecipientViewModels;
using System.Linq;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Recipients, include all methods needed to work with Recipient storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class RecipientManager : BaseManager, IRecipientManager
    {
        public RecipientManager(IUnitOfWork unitOfWork , IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Delete recipient by Id
        /// </summary>
        /// <param name="id">Id of recipient wich need to delete</param>
        public bool Delete(int id)
        {
                Recipient recipient = unitOfWork.Recipients.GetById(id);
                if (recipient == null)
                {
                    return false;
                }
                unitOfWork.Recipients.Delete(recipient);
                unitOfWork.Save();
                return true;
        }

        public IEnumerable<RecipientViewModel> GetRecipients(int companyId, int page, int countOnPage, string searchValue)
        {
            IEnumerable<Recipient> Recipients = unitOfWork.Recipients.Get(ec => (ec.CompanyId == companyId)&& (
                ec.Name.Contains(searchValue)    || 
                ec.Surname.Contains(searchValue) ||
                ec.KeyWords.Contains(searchValue)
                ))
                .Skip((page - 1) * countOnPage).Take(countOnPage);
            foreach (var recipient in Recipients)
            {
                recipient.Phone = unitOfWork.Phones.GetById(recipient.PhoneId);
            }
            return mapper.Map<IEnumerable<Recipient>, IEnumerable<RecipientViewModel>>(Recipients);
        }

        public int GetRecipientsCount(int companyId, string searchValue)
        {
            return unitOfWork.Recipients.Get(ec => (ec.CompanyId == companyId) &&( ec.Name.Contains(searchValue))).Count();
        }

        /// <summary>
        /// Get one recipient from db by Id
        /// </summary>
        /// <param name="id">Id of recipient wich you need</param>
        /// <returns>ViewModel of recipient from db</returns>
        public RecipientViewModel GetRecipientById(int id)
        {
            Recipient recipient = unitOfWork.Recipients.GetById(id);
            recipient.Phone = unitOfWork.Phones.GetById(recipient.PhoneId);
            return mapper.Map<Recipient, RecipientViewModel>(recipient);
        }



        /// <summary>
        /// Method for getting all recipients which belong to specified company
        /// </summary>
        /// <param name="companyId">Id of company wich belongs needed recipients</param>
        /// <returns>IEnumerable of mapped to ViewModel objects</returns>
        public IEnumerable<RecipientViewModel> GetRecipients(int companyId)
        {
            IEnumerable<Recipient> recipients = unitOfWork.Recipients.Get(r => r.CompanyId == companyId);
            foreach (var rec in recipients)
            {
                rec.Phone = unitOfWork.Phones.GetById(rec.PhoneId);
            }
            return mapper.Map<IEnumerable<Recipient>, IEnumerable<RecipientViewModel>>(recipients);
        }


        /// <summary>
        /// Method for inserting new recipient to db, and check Phone table, if phone doesn't exist - adding it to Phones
        /// </summary>
        /// <param name="item">ViewModel of recipient</param>
        /// <param name="companyId">Id of company wich belongs this recipient</param>
        public bool Insert(RecipientViewModel item, int companyId)
        {
                Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
                recipient.CompanyId = companyId;
                List<Phone> phone = unitOfWork.Phones.Get(p => p.PhoneNumber == item.Phonenumber).ToList();
                if (phone.Count == 0)
                {
                    Phone newPhone = new Phone();
                    newPhone.PhoneNumber = item.Phonenumber;
                    unitOfWork.Phones.Insert(newPhone);
                    unitOfWork.Save();
                    recipient.Phone = newPhone;
                }
                else
                {
                    recipient.Phone = phone[0];
                }
                unitOfWork.Recipients.Insert(recipient);
                unitOfWork.Save();
                return true;
        }

        /// <summary>
        /// Method for updating recipient in db, and check Phone table, if phone doesn't exist - adding it to Phones
        /// </summary>
        /// <param name="item">ViewModel of recipient</param>
        public bool Update(RecipientViewModel item)
        {
                Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
                List<Phone> phone = unitOfWork.Phones.Get(p => p.PhoneNumber == item.Phonenumber).ToList();
                if (phone.Count == 0)
                {
                    Phone newPhone = new Phone();
                    newPhone.PhoneNumber = item.Phonenumber;
                    unitOfWork.Phones.Insert(newPhone);
                    unitOfWork.Save();
                    recipient.Phone = newPhone;
                }
                else
                {
                    recipient.PhoneId = phone[0].Id;
                }
                unitOfWork.Recipients.Update(recipient);
                unitOfWork.Save();
                return true;
        }
    }
}
