using AutoMapper;
using BAL.Interfaces;
using Model.DTOs;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using System.Linq;
using BAL.Exceptions;
using Model.ViewModels.RecievedMessageViewModel;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for RecievedMessage
    /// </summary>
    public class RecievedMessageManager : BaseManager, IRecievedMessageManager
    {
        public RecievedMessageManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Get Recieved Messages by id
        /// </summary>
        /// <param name="id">id of recieved message</param>
        /// <returns>RecievedMessageViewModel</returns>
        public RecievedMessageViewModel Get(int id)
        {
            RecievedMessage recievedMessage = unitOfWork.RecievedMessages.GetById(id);
            return mapper.Map<RecievedMessage, RecievedMessageViewModel>(recievedMessage);
        }

        /// <summary>
        /// Get list of Recieved Messages
        /// </summary>
        /// <param name="companyId">id of company</param>
        /// <returns>List of RecievedMessagesViewModel</returns>
        public IEnumerable<RecievedMessageViewModel> GetRecievedMessages(int companyId)
        {
            IEnumerable<RecievedMessage> recievedMessages = unitOfWork.RecievedMessages
                .Get(filter: c => c.CompanyId == companyId);
            foreach (var rc in recievedMessages)
            {
                Company company = unitOfWork.Companies.GetById(rc.CompanyId);
                if (company.PhoneId != null)
                {
                    company.Phone = unitOfWork.Phones.GetById((int) company.PhoneId);
                    rc.Company = company;
                    rc.Phone = unitOfWork.Phones.GetById(rc.PhoneId);
                }
            }
            return mapper.Map<IEnumerable<RecievedMessage>, IEnumerable<RecievedMessageViewModel>>(recievedMessages);
        }

        /// <summary>
        /// insert new Recieved Message in bd
        /// </summary>
        /// <param name="item">RecievedMessageDTO for inserting</param>
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
        
        /// <summary>
        /// Delete Recieved Message from db
        /// </summary>
        /// <param name="id">id of recieved message for deleting</param>
        public void Delete(int id)
        {
            try
            {
                RecievedMessage recievedMessage = unitOfWork.RecievedMessages.GetById(id);
                unitOfWork.RecievedMessages.Delete(recievedMessage);
                unitOfWork.Save();
            }
            catch
            {
               throw new  TypeAccessException();
            }
        }

        /// <summary>
        /// check who is in the reported subscribeword
        /// if there is something 
        /// look for the recipient and block the mailing in this group or start
        /// </summary>
        /// <param name="Originator">recipient PhoneNumber</param>
        /// <param name="Destination">company PhoneNumber</param>
        /// <param name="Content">message that came back</param>
        /// <param name="message">message.Originator/Destination/Content</param>

        public void SearchSubscribeWordInMessages(RecievedMessageDTO message)
        {
            SubscribeWord subscribeWord = unitOfWork.SubscribeWords.GetAll().FirstOrDefault(c => c.Word == message.MessageText);

            if (subscribeWord == null)
            {
                return;
            }
           /*     Phone orignator;
                Phone destination;
                Company company;
                try
                {
                    orignator = unitOfWork.Phones.GetAll()
                        .FirstOrDefault(item => item.PhoneNumber == message.SenderPhone);
                    //Get(item => item.PhoneNumber == message.SenderPhone).FirstOrDefault();

                    destination = unitOfWork.Phones.GetAll()
                        .FirstOrDefault(item => item.PhoneNumber == message.RecipientPhone);
                    // Get(item => item.PhoneNumber == message.RecipientPhone).FirstOrDefault();
                    company = unitOfWork.Companies.Get(item => item.PhoneId == destination.Id).FirstOrDefault();
                    if (orignator == null)
                    {
                        orignator = new Phone() { PhoneNumber = message.SenderPhone };
                        unitOfWork.Phones.Insert(orignator);
                        unitOfWork.Save();
                    }
                }
                catch
                {
                 return;
                }

                if ((company == null) || (subscribeWord.CompanyId != company.Id))
                {
                 return;
                }


                if (subscribeWord.SubscribePhoneId == null)
                {
                 return;
                }

                    Phone subscribeCompanyPhone = unitOfWork.Phones.GetById((int) subscribeWord.SubscribePhoneId);
                    var subscribeCompanies = unitOfWork.Companies
                        .Get(item => item.PhoneId == subscribeCompanyPhone.Id);

                    if (subscribeCompanyPhone == orignator)
                    {
                    return;
                    }

                    foreach (var subscribeCompany in subscribeCompanies)
                    {


                        var rec = unitOfWork.Recipients.GetAll()
                            .FirstOrDefault(r =>(r.CompanyId == subscribeCompany.Id && r.PhoneId == orignator.Id));

                        if (rec == null)
                        {
                            unitOfWork.Recipients.Insert(new Recipient()
                            {
                                CompanyId = subscribeCompany.Id,
                                PhoneId = orignator.Id,
                                KeyWords = "Subscribed himself",
                            });
                        }
                    }

                    unitOfWork.Save();
              */
        }

        /// <summary>
        /// check who is in the reported stopword 
        /// if there is something 
        /// look for the recipient and block the mailing in this group or start
        /// </summary>
        /// <param name="Originator">recipient PhoneNumber</param>
        /// <param name="Destination">company PhoneNumber</param>
        /// <param name="Content">message that came back</param>
        /// <param name="message">message.Originator/Destination/Content</param>
        public void SearchStopWordInMessages(RecievedMessageDTO message)
        {
            StopWord words = unitOfWork.StopWords.GetAll().FirstOrDefault(c =>
                c.Word == message.MessageText);

            if (words == null)
            {
                return;
            }

            Phone orignator;
            Phone destination;
            Company company;
            try
            {
                orignator = unitOfWork.Phones.GetAll()
                    .FirstOrDefault(item => item.PhoneNumber == message.SenderPhone);
                //Get(item => item.PhoneNumber == message.SenderPhone).FirstOrDefault();
                destination = unitOfWork.Phones.GetAll()
                    .FirstOrDefault(item => item.PhoneNumber == message.RecipientPhone);
                //Get(item => item.PhoneNumber == message.RecipientPhone).FirstOrDefault();
                company = unitOfWork.Companies.Get(item => item.PhoneId == destination.Id).FirstOrDefault();
            }
            catch
            {
            return;
            }
        if ((words.Word == "START") && (orignator != null) && (company != null))
            {
                try
                {
                    PhoneGroupUnsubscribe phoneGroup = unitOfWork.PhoneGroupUnsubscribes.GetAll().FirstOrDefault(w =>
                        ((w.GroupId == company.ApplicationGroupId) && (w.PhoneId == orignator.Id)));
                    if (phoneGroup != null)
                    {
                        unitOfWork.PhoneGroupUnsubscribes.Delete(phoneGroup);
                        unitOfWork.Save();
                        //return true;
                    }
                }
                catch
                {
                    return;
                }

            }
            else
            {
                if ((orignator == null)||(company==null))
                {
                 return;

                }


                PhoneGroupUnsubscribe phoneGroup = new PhoneGroupUnsubscribe()
                    {GroupId = company.ApplicationGroupId, PhoneId = orignator.Id};

                unitOfWork.PhoneGroupUnsubscribes.Insert(phoneGroup);
                unitOfWork.Save();
               
            }
        }

    
    }

        
}

