using Model.ViewModels.ContactViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public interface IContactManager
    {
        ContactViewModel GetContact(int ContactId);
        List<ContactViewModel> GetContact(string userId, int pageNumber, int pageSize);
        List<ContactViewModel> GetContactBySearchValue(string userId ,int pageNumber, int pageSize, 
            string searchValue);
        int GetContactCount( string userId);
        int GetContactBySearchValueCount(string userId, string searchValue);
        bool CreateContact(ContactViewModel contactModel, string userId);
        void DeleteContact(int id);
        bool UpdateContact(ContactViewModel contactModel, string userId);
    }
}
