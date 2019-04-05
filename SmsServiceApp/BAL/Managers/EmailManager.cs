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

        public Email GetEmailById(int Id)
        {
            return unitOfWork.Emails.GetById(Id);
        }

        public string GetEmailText(int Id)
        {
            return unitOfWork.Emails.GetById(Id).EmailAddress;
        }

        public int GetEmailId(string email)
        {
            return unitOfWork.Emails.GetAll().FirstOrDefault(p => p.EmailAddress == email).Id;
        }

        public IEnumerable<Email> GetEmails()
        {
            return unitOfWork.Emails.GetAll();
        }

        public void Insert(Email item)
        {
            unitOfWork.Emails.Insert(item);
            unitOfWork.Save();
        }

        public bool IsEmailExist(string email)
        {
            var _email = unitOfWork.Emails.GetAll().FirstOrDefault(p => p.EmailAddress == email);
            if (_email == null)
            {
                return false;
            }
            return true;
        }
    }
}
