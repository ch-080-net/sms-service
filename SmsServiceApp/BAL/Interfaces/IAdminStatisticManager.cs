using Model.ViewModels.AdminStatisticViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IAdminStatisticManager
    {
        IEnumerable<AdminStatisticViewModel> GetAll();
    }
}
