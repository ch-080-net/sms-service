using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.GroupViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class GroupManager : BaseManager, IGroupManager
    {
        public GroupManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public GroupViewModel Get(int id)
        {
            ApplicationGroup group = unitOfWork.ApplicationGroups.GetById(id);
            return mapper.Map<ApplicationGroup, GroupViewModel>(group);
        }

        public IEnumerable<GroupViewModel> GetGroups()
        {
            IEnumerable<ApplicationGroup> groups = unitOfWork.ApplicationGroups.GetAll();
            return mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<GroupViewModel>>(groups);
        }

        public void Insert(GroupViewModel item)
        {
            ApplicationGroup group = mapper.Map<GroupViewModel, ApplicationGroup>(item);
            unitOfWork.ApplicationGroups.Insert(group);
            unitOfWork.Save();
        }

        public void Update(GroupViewModel item)
        {
            ApplicationGroup group = mapper.Map<GroupViewModel, ApplicationGroup>(item);
            unitOfWork.ApplicationGroups.Update(group);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            ApplicationGroup group = unitOfWork.ApplicationGroups.GetById(id);
            unitOfWork.ApplicationGroups.Delete(group);
            unitOfWork.Save();
        }
    }
}
