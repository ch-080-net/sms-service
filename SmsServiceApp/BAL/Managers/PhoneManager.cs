using AutoMapper;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class PhoneManager : BaseManager, IPhoneManager
    {
        public PhoneManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public Phone GetPhoneById(int Id)
        {
            return unitOfWork.Phones.GetById(Id);
        }

        public IEnumerable<Phone> GetPhones()
        {
            return unitOfWork.Phones.GetAll();
        }
    }
}
