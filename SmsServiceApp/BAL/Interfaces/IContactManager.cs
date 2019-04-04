using Microsoft.AspNetCore.Http;
using Model.ViewModels.ContactViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Interface with CRUD operation for Contacts
    /// </summary>
    public interface IContactManager
    {
        ContactViewModel GetContact(int ContactId);
        List<ContactViewModel> GetContact(int groupId, int pageNumber, int pageSize);
        List<ContactViewModel> GetContactBySearchValue(int groupId ,int pageNumber, int pageSize, 
            string searchValue);
        int GetContactCount(int groupId);
        int GetContactCountBySearchValue(int groupId, string searchValue);
        bool CreateContact(ContactViewModel contactModel, int groupId);
        void DeleteContact(int id);
        bool UpdateContact(ContactViewModel contactModel, int groupId);
        void AddContactFromFile(IFormFile file, string Id);
    }
}
