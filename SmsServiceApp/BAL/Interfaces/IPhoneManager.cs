using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Interface with CRUD operation for Phones
    /// </summary>
    public interface IPhoneManager
    {
        IEnumerable<Phone> GetPhones();
        Phone GetPhoneById(int Id);
        string GetPhoneNumber(int Id);
        void Insert(Phone item);
        bool IsPhoneNumberExist(string phoneNumber);
        int GetPhoneId(string number);
    }
}
