using AutoMapper;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Managers
{
    class PhoneManager : BaseManager/*, IPhoneManager*/
    {
        public PhoneManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        //IEnumerable<Phone> GetPhones()
        //{

        //}
    }
}
