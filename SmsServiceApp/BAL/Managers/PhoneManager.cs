using AutoMapper;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

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

        public string GetPhoneNumber(int Id)
        {
            return unitOfWork.Phones.GetById(Id).PhoneNumber;
        }

        public int GetPhoneId(string number)
        {
            return unitOfWork.Phones.GetAll().FirstOrDefault(p => p.PhoneNumber == number).Id;
        }

        public IEnumerable<Phone> GetPhones()
        {
            return unitOfWork.Phones.GetAll();
        }
    }
}
