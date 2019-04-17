using AutoMapper;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Phones, include all methods needed to work with Phone storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class PhoneManager : BaseManager, IPhoneManager
    {
        public PhoneManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        /// <summary>
        /// Get Phone by Id from db
        /// </summary>
        /// <param name="Id">PhoneId</param>
        /// <returns>Phone entity from db</returns>
        public Phone GetPhoneById(int Id)
        {
            return unitOfWork.Phones.GetById(Id);
        }

        /// <summary>
        /// Get Phone number by phone Id
        /// </summary>
        /// <param name="Id">PhoneId</param>
        /// <returns>PhoneNumber</returns>
        public string GetPhoneNumber(int Id)
        {
            return unitOfWork.Phones.GetById(Id).PhoneNumber;
        }

        /// <summary>
        /// Get All Phones from db
        /// </summary>
        /// <returns>Collection of all phones</returns>
        public IEnumerable<Phone> GetPhones()
        {
            return unitOfWork.Phones.GetAll();
        }

        /// <summary>
        /// Insert Phone to db
        /// </summary>
        /// <param name="item">Phone entity</param>
        public void Insert(Phone item)
        {
                unitOfWork.Phones.Insert(item);
                unitOfWork.Save();
        }
    }
}
