using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public interface IContactManager
    {
        IEnumerable<Contact> GetContact(int pageNumber, int pageSize);
        IEnumerable<Contact> GetContactByName(int pageNumber, int pageSize, 
            string searchValue);
        //IEnumerable<Contact> GetContactByPhoneNumber(int pageNumber, int pageSize,
        //    string searchValue);
    }
}
