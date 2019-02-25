using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using AutoMapper;
using Model.ViewModels.RecipientViewModels;
using System.Linq;

namespace BAL.Managers
{
    public class RecipientManager : BaseManager, IRecipientManager
    {
        public RecipientManager(IUnitOfWork unitOfWork , IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public void Delete(int id)
        {
            Recipient recipient = unitOfWork.Recipients.GetById(id);
            unitOfWork.Recipients.Delete(recipient);
            unitOfWork.Save();
        }

        public RecipientViewModel GetRecipientById(int id)
        {
            Recipient recipient = unitOfWork.Recipients.GetById(id);
            recipient.Phone = unitOfWork.Phones.GetById(recipient.PhoneId);
            return mapper.Map<Recipient, RecipientViewModel>(recipient);
        }

        public IEnumerable<RecipientViewModel> GetRecipients(int companyId)
        {
            IEnumerable<Recipient> recipients = unitOfWork.Recipients.GetAll().Where(r => r.CompanyId == companyId);
            foreach (var rec in recipients)
            {
                rec.Phone = unitOfWork.Phones.GetById(rec.PhoneId);
            }
            return mapper.Map<IEnumerable<Recipient>, List<RecipientViewModel>>(recipients);
        }

        public void Insert(RecipientViewModel item, int companyId)
        {
            Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
            recipient.CompanyId = companyId;
            List<Phone> phone = unitOfWork.Phones.Get(p => p.PhoneNumber == item.PhoneNumber).ToList();
            if (phone.Count == 0)
            {
                Phone newPhone = new Phone();
                newPhone.PhoneNumber = item.PhoneNumber;
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
        }

        public void Update(RecipientViewModel item)
        {
            Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
            List<Phone> phone = unitOfWork.Phones.Get(p => p.PhoneNumber == item.PhoneNumber).ToList();
            if (phone.Count == 0)
            {
                Phone newPhone = new Phone();
                newPhone.PhoneNumber = item.PhoneNumber;
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
        }
    }
}
