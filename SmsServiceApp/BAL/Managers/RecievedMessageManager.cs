using AutoMapper;
using BAL.Interfaces;
using Model.DTOs;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using System.Linq;

namespace BAL.Managers
{
    public class RecievedMessageManager : BaseManager, IRecievedMessageManager
    {
        public RecievedMessageManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public RecievedMessageDTO Get(int id)
        {
            RecievedMessage recievedMessage = unitOfWork.RecievedMessages.GetById(id);
            return mapper.Map<RecievedMessage, RecievedMessageDTO>(recievedMessage);
        }

        public IEnumerable<RecievedMessageDTO> GetRecievedMessages(int companyId)
        {
            IEnumerable<RecievedMessage> recievedMessages = unitOfWork.RecievedMessages
                .Get(filter: c => c.CompanyId == companyId);
            return mapper.Map<IEnumerable<RecievedMessage>, IEnumerable<RecievedMessageDTO>>(recievedMessages);
        }

        public void Insert(RecievedMessageDTO item)
        {
            RecievedMessage recievedMessage = mapper.Map<RecievedMessageDTO, RecievedMessage>(item);
            unitOfWork.Phones.Get(filter: p => p.PhoneNumber == item.SenderPhone);
            List<Phone> phone = unitOfWork.Phones.Get(filter: p => p.PhoneNumber == item.SenderPhone).ToList();
            if (phone.Count == 0)
            {
                Phone newPhone = new Phone();
                newPhone.PhoneNumber = item.SenderPhone;
                unitOfWork.Phones.Insert(newPhone);
                unitOfWork.Save();
                recievedMessage.Phone = newPhone;
            }
            else
            {
                recievedMessage.Phone = phone[0];
            }
            phone = unitOfWork.Phones.Get(filter: p => p.PhoneNumber == item.RecipientPhone).ToList();
            if (phone.Count == 0)
            {
                return;
            }
            else
            {
                List<Company> company = unitOfWork.Companies.Get(filter: c => c.PhoneId == phone[0].Id).ToList();
                if (company.Count == 0)
                {
                    return;
                }
                else
                {
                    recievedMessage.Company = company[0];
                }
            }
            unitOfWork.RecievedMessages.Insert(recievedMessage);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            RecievedMessage recievedMessage = unitOfWork.RecievedMessages.GetById(id);
            unitOfWork.RecievedMessages.Delete(recievedMessage);
            unitOfWork.Save();
        }
    }
}
