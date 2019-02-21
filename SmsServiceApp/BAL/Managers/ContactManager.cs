using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using System.Linq.Expressions;

namespace BAL.Managers
{
    public class ContactManager : BaseManager, IContactManager
    {
        public ContactManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<Contact> GetContact(int pageNumber, int pageSize)
        {
            return unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize);
        }

        public IEnumerable<Contact> GetContactByName(int pageNumber, int pageSize,
            string searchValue)
        {
            return unitOfWork.Contacts.GetContactsByPageNumber(pageNumber, pageSize,
                    filter: item => item.Name == searchValue || item.Surname == searchValue ||
                    item.Name + " " + item.Surname == searchValue);

        }
    }
}
