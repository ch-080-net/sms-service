using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using System.Linq.Expressions;
using System.Linq;
using AutoMapper;
using Model.ViewModels.ContactViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Contacts, include all methods needed to work with Contact storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class ContactManager : BaseManager, IContactManager
    {   

        public ContactManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Method for getting Contact by id
        /// </summary>
        /// <param name="ContactId">Id of contact</param>
        /// <returns>View model of Contact</returns>
        public ContactViewModel GetContact(int ContactId)
        {
            return mapper.Map<ContactViewModel>(unitOfWork.Contacts.GetById(ContactId));
        }

        /// <summary>
        /// Method for getting all Contacts which belong to specified group on current page
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <param name="pageNumber">Number of current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <returns>List with view models of contacts</returns>
        public List<ContactViewModel> GetContact(int groupId, int pageNumber, int pageSize)
        {
                var contacts = unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize,
                    filter: item => item.ApplicationGroupId == groupId);
                foreach (var contact in contacts)
                {
                    contact.Phone = unitOfWork.Phones.GetById(contact.PhoneId);
                }
                return mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
        }

        /// <summary>
        /// Method for getting all Contacts which belong to specified group on current page with search value
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <param name="pageNumber">Number of current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchValue">Search value</param>
        /// <returns>List with view models of contacts</returns>
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

        /// <summary>
        /// Method for getting count of contacts that belong to group
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for getting count of contacts that belong to group with search value
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <param name="searchValue">Search value</param>
        /// <returns></returns>
        public int GetContactCountBySearchValue(int groupId, string searchValue)
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

        /// <summary>
        /// Method for inserting new company to db
        /// </summary>
        /// <param name="contactModel">View model of contact</param>
        /// <param name="groupId">Id of Group wich create this contact</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete Contact by Id
        /// </summary>
        /// <param name="id">Id of contact</param>
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

        /// <summary>
        /// Update Company in db
        /// </summary>
        /// <param name="contactModel">View model of contact</param>
        /// <param name="groupId">Id of Group wich update this contact</param>
        /// <returns></returns>
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

        public void AddContactFromFile(IFormFile file)
        {
            var result = string.Empty;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                result = reader.ReadToEnd();
            }

            var contacts = TranslateToContacts(result);
            foreach (var temp in contacts)
            {
                unitOfWork.Contacts.Insert(temp);
            }

            unitOfWork.Save();
            
        }

        private List<Contact> TranslateToContacts(string contacts)
        {
            var splitedContacts = string.IsNullOrEmpty(contacts) ? null : contacts.Split(',').ToList();
            var result = new List<Contact>();
            for (int i = 0; i < splitedContacts.Count/3; i+=3)
            {
                var temp=new Contact();
                temp.Phone = new Phone() {PhoneNumber = splitedContacts.ElementAt(i)};
                temp.BirthDate = Convert.ToDateTime(splitedContacts.ElementAt(i + 1));
                temp.Gender = Convert.ToByte(splitedContacts.ElementAt(i+2));
                result.Add(temp);
            }

            return result;
        }

       
    }
}
