using Model.ViewModels.GroupViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Managers
{
    /// <summary>
    /// Interface with CRUD operation for Groups
    /// </summary>
    public interface IGroupManager
    {
        GroupViewModel Get(int id);
        IEnumerable<GroupViewModel> GetGroups();
        bool Insert(GroupViewModel item);
        bool Update(GroupViewModel item);
        bool Delete(int id);
    }
}
