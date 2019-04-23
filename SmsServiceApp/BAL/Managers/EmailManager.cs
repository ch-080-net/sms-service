using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    public class EmailManager : BaseManager, IEmailManager
    {
        public EmailManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public Email GetEmailById(int id)
        {
            try
            {
                return unitOfWork.Emails.GetById(id);
            }
            catch (Exception e)
            {
                throw e;
            }           
        }

        public string GetEmailAddress(int id)
        {
            try
            {
                return unitOfWork.Emails.GetById(id).EmailAddress;
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        public int GetEmailId(string email)
        {
            try
            {
                return unitOfWork.Emails.GetAll().FirstOrDefault(p => p.EmailAddress == email).Id;
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        public IEnumerable<Email> GetEmails()
        {
            return unitOfWork.Emails.GetAll();
        }

        public void Insert(Email item)
        {
            try
            {
                unitOfWork.Emails.Insert(item);
                unitOfWork.Save();
            }
            catch (Exception e)
            {
                throw e;
            }           
        }

        public bool IsEmailExist(string email)
        {
            try
            {
                var _email = unitOfWork.Emails.GetAll().FirstOrDefault(p => p.EmailAddress == email);
                if (_email == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
