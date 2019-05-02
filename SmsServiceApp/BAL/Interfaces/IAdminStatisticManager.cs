using Model.ViewModels.AdminStatisticViewModel;
using System.Collections.Generic;


namespace BAL.Interfaces
{
    public interface IAdminStatisticManager
    {
        IEnumerable<AdminStatisticViewModel> NumberOfMessages();
    }
}
