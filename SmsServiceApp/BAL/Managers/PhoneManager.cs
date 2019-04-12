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
        /// Get Phone Id by number
        /// </summary>
        /// <param name="number">PhoneNumber</param>
        /// <returns>PhoneId</returns>
        public int GetPhoneId(string number)
        {
            return unitOfWork.Phones.GetAll().FirstOrDefault(p => p.PhoneNumber == number).Id;
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

        /// <summary>
        /// Check if phone already exist
        /// </summary>
        /// <param name="phoneNumber">phone number</param>
        /// <returns>Is number exist</returns>
        public bool IsPhoneNumberExist(string phoneNumber)
        {
            var phone = unitOfWork.Phones.GetAll().FirstOrDefault(p => p.PhoneNumber == phoneNumber);
            if(phone == null)
            {
                return false;
            }
            return true;
        }
    }
}
