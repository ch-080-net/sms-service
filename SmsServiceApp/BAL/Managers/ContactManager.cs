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

        public List<ContactViewModel> GetContact(int groupId, int pageNumber, int pageSize)
        {
            try
            {
                var contacts = unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize,
                    filter: item => item.ApplicationGroupId == groupId);
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

        public List<ContactViewModel> GetContactBySearchValue(int groupId, int pageNumber, int pageSize,
            string searchValue)
        {
            try
            {
                var contacts = unitOfWork.Contacts.GetAll();
                foreach (var contact in contacts)
                {
                    contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
                }
                contacts = contacts.Where(item => item.ApplicationGroupId == groupId &&
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

        public int GetContactCount(int groupId)
        {
            try
            {
                List<Contact> contacts = unitOfWork.Contacts.Get(
                    filter: item => item.ApplicationGroupId == groupId).ToList();
                return contacts.Count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int GetContactBySearchValueCount(int groupId, string searchValue)
        {
                List<Contact> contacts = unitOfWork.Contacts.Get(
                        filter: item => item.ApplicationGroupId == groupId).ToList();
                foreach (var contact in contacts)
                {
                    contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
                }
                contacts = contacts.Where(item => item.Phone.PhoneNumber == searchValue ||
                        item.Name == searchValue || item.Surname == searchValue ||
                        item.Name + " " + item.Surname == searchValue || item.KeyWords.Contains(searchValue)).ToList();

                return contacts.Count;
        }

        public bool CreateContact(ContactViewModel contactModel, int groupId)
        {
            try
            {
                Contact newContact = mapper.Map<Contact>(contactModel);
                newContact.ApplicationGroupId = groupId;
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

        public bool UpdateContact(ContactViewModel contactModel, int groupId)
        {
            try
            {
                Contact contact = mapper.Map<Contact>(contactModel);
                contact.ApplicationGroupId = groupId;
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
