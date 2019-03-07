using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    public interface IPhoneManager
    {
        IEnumerable<Phone> GetPhones();
        Phone GetPhoneById(int Id);
    }
}
