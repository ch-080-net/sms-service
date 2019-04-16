using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.RecipientViewModels;

namespace BAL.Managers
{
    /// <summary>
    /// Interface with CRUD operation for Recipients
    /// </summary>
    public interface IRecipientManager
    {
        IEnumerable<RecipientViewModel> GetRecipients(int companyId);
        IEnumerable<RecipientViewModel> GetRecipients(int companyId, int page, int countOnPage, string searchValue);
        int GetRecipientsCount(int companyId, string searchValue);
        RecipientViewModel GetRecipientById(int id);
        bool Insert(RecipientViewModel item, int companyId);
        bool Update(RecipientViewModel item);
        bool Delete(int id);
    }
}
