using Model.ViewModels.ContactViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public interface IContactManager
    {
        List<ContactViewModel> GetContact(int pageNumber, int pageSize);
        List<ContactViewModel> GetContactByName(int pageNumber, int pageSize, 
            string searchValue);
        int GetContactCount();
        int GetContactByNameCount(string searchValue);
        //IEnumerable<Contact> GetContactByPhoneNumber(int pageNumber, int pageSize,
        //    string searchValue);
    }
}
