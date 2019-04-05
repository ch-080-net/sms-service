using Model.ViewModels.EmailRecipientViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IEmailRecipientManager
    {
        List<EmailRecipientViewModel> GetEmailRecipients(int companyId);
        EmailRecipientViewModel GetEmailRecipientById(int id);
        void Insert(EmailRecipientViewModel item, int companyId);
        void Update(EmailRecipientViewModel item);
        void Delete(int id);
    }
}
