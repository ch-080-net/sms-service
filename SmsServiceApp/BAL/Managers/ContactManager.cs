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

        public List<ContactViewModel> GetContact(string userId, int pageNumber, int pageSize)
        {
            var contacts = unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize, 
                filter: item => item.ApplicationUserId == userId);
            foreach (var contact in contacts)
            {
                contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
            }
            return mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
        }

        public List<ContactViewModel> GetContactBySearchValue(string userId, int pageNumber, int pageSize,
            string searchValue)
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

        public int GetContactCount(string userId)
        {
            List<Contact> contacts = unitOfWork.Contacts.Get(
                filter: item => item.ApplicationUserId == userId).ToList();
            return contacts.Count;
        }

        public int GetContactBySearchValueCount(string userId, string searchValue)
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

        public void CreateContact(ContactViewModel contactModel, string userId)
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
            unitOfWork.Contacts.Insert(contact);
            unitOfWork.Save();
        }

        public void DeleteContact(int id)
        {
            Contact contact = unitOfWork.Contacts.GetById(id);
            unitOfWork.Contacts.Delete(contact);
            unitOfWork.Save();
        }

        public void UpdateContact(ContactViewModel contactModel, string userId)
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
        }
    }
}
