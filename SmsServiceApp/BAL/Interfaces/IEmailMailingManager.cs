using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IEmailMailingManager
    {
        IEnumerable<EmailDTO> GetUnsentEmails();

        void MarkAs(EmailDTO messages, byte messageState);
    }
}
