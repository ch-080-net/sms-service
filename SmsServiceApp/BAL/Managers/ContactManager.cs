using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using System.Linq.Expressions;
using AutoMapper;
using Model.ViewModels.ContactViewModels;

namespace BAL.Managers
{
    public class ContactManager : BaseManager, IContactManager
    {
        public ContactManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public List<ContactViewModel> GetContact(int pageNumber, int pageSize)
        {
            var contacts = unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize);
            foreach (var contact in contacts)
            {
                contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
            }
            return mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
        }

        public List<ContactViewModel> GetContactByName(int pageNumber, int pageSize,
            string searchValue)
        {
            var contacts = unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize,
                    filter: item => item.Name == searchValue || item.Surname == searchValue ||
                    item.Name + " " + item.Surname == searchValue);
            foreach (var contact in contacts)
            {
                contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
            }
            return mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
        }

        public int GetContactCount()
        {
            List<ContactViewModel> contacts = mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(
                unitOfWork.Contacts.GetAll());
            return contacts.Count;
        }

        public int GetContactByNameCount(string searchValue)
        {
            List<ContactViewModel> contacts = mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(
                unitOfWork.Contacts.Get(
                    filter: item => item.Name == searchValue || item.Surname == searchValue ||
                     item.Name + " " + item.Surname == searchValue));
            return contacts.Count;
        }
    }
}
