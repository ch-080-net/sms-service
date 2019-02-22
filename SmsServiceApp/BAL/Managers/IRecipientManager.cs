using WebCustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.RecipientViewModels;

namespace BAL.Managers
{
    public interface IRecipientManager
    {
        IEnumerable<RecipientViewModel> GetRecipients();
        void Insert(RecipientViewModel item);
        void Update(RecipientViewModel item);
        void Delete(RecipientViewModel item);
        void SetStateModified(RecipientViewModel item);
    }
}
