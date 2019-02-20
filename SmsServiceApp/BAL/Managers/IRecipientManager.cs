using WebCustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Managers
{
    public interface IRecipientManager
    {
        IEnumerable<Recipient> GetRecipients();
    }
}
