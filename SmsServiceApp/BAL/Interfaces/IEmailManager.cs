using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Interfaces
{
    public interface IEmailManager
    {
        IEnumerable<Email> GetEmails();
        Email GetEmailById(int id);
        string GetEmailText(int id);
        void Insert(Email item);
        bool IsEmailExist(string email);
        int GetEmailId(string email);
    }
}
