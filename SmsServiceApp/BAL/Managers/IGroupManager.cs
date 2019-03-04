using Model.ViewModels.GroupViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Managers
{
    public interface IGroupManager
    {
        GroupViewModel Get(int id);
        IEnumerable<GroupViewModel> GetGroups();
        void Insert(GroupViewModel item);
        void Update(GroupViewModel item);
        void Delete(int id);
    }
}
