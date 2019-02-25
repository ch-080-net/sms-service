using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using System.Linq.Expressions;
using System.Linq;
using AutoMapper;
using Model.ViewModels.ContactViewModels;

namespace BAL.Managers
{
    public class ContactManager : BaseManager, IContactManager
    {
        public ContactManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public ContactViewModel GetContact(int ContactId)
        {
            return mapper.Map<ContactViewModel>(unitOfWork.Contacts.GetById(ContactId));
        }

        public List<ContactViewModel> GetContact(string userId, int pageNumber, int pageSize)
        {
            try
            {
                var contacts = unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize,
                    filter: item => item.ApplicationUserId == userId);
                foreach (var contact in contacts)
                {
                    contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
                }
                return mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactViewModel> GetContactBySearchValue(string userId, int pageNumber, int pageSize,
            string searchValue)
        {
            try
            {
                var contacts = unitOfWork.Contacts.GetAll();
                foreach (var contact in contacts)
                {
                    contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
                }
                contacts = contacts.Where(item => item.ApplicationUserId == userId &&
                        (item.Phone.PhoneNumber == searchValue ||
                        item.Name == searchValue || item.Surname == searchValue ||
                        item.Name + " " + item.Surname == searchValue || item.KeyWords.Contains(searchValue)))
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetContactCount(string userId)
        {
            try
            {
                List<Contact> contacts = unitOfWork.Contacts.Get(
                    filter: item => item.ApplicationUserId == userId).ToList();
                return contacts.Count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int GetContactBySearchValueCount(string userId, string searchValue)
        {
            try
            {
                List<Contact> contacts = unitOfWork.Contacts.Get(
                        filter: item => item.ApplicationUserId == userId).ToList();
                foreach (var contact in contacts)
                {
                    contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
                }
                contacts = contacts.Where(item => item.Phone.PhoneNumber == searchValue ||
                        item.Name == searchValue || item.Surname == searchValue ||
                        item.Name + " " + item.Surname == searchValue || item.KeyWords.Contains(searchValue)).ToList();

                return contacts.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateContact(ContactViewModel contactModel, string userId)
        {
            try
            {
                Contact newContact = mapper.Map<Contact>(contactModel);
                newContact.ApplicationUserId = userId;
                List<Phone> phone = unitOfWork.Phones.Get
                    (filter: item => item.PhoneNumber == contactModel.PhonePhoneNumber).ToList();
                if (phone.Count == 0)
                {
                    Phone newPhone = new Phone();
                    newPhone.PhoneNumber = contactModel.PhonePhoneNumber;
                    unitOfWork.Phones.Insert(newPhone);
                    unitOfWork.Save();
                    newContact.Phone = newPhone;
                }
                else
                {
                    List<Contact> contact = unitOfWork.Contacts.Get(filter: item => item.PhoneId == phone[0].Id).ToList();
                    if (contact.Count != 0)
                    {
                        return false;
                    }
                    newContact.Phone = phone[0];
                }
                unitOfWork.Contacts.Insert(newContact);
                unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContact(int id)
        {
            try
            {
                Contact contact = unitOfWork.Contacts.GetById(id);
                unitOfWork.Contacts.Delete(contact);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateContact(ContactViewModel contactModel, string userId)
        {
            try
            {
                Contact contact = mapper.Map<Contact>(contactModel);
                contact.ApplicationUserId = userId;
                List<Phone> phone = unitOfWork.Phones.Get(filter: item => item.PhoneNumber == contactModel.PhonePhoneNumber).ToList();
                if (phone.Count == 0)
                {
                    Phone newPhone = new Phone();
                    newPhone.PhoneNumber = contactModel.PhonePhoneNumber;
                    unitOfWork.Phones.Insert(newPhone);
                    unitOfWork.Save();
                    contact.Phone = newPhone;
                }
                else
                {
                    contact.Phone = phone[0];
                }
                unitOfWork.Contacts.SetStateModified(contact);
                unitOfWork.Contacts.Update(contact);
                unitOfWork.Save();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
