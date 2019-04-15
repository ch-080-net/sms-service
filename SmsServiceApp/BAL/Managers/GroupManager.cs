using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.GroupViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Groups, include all methods needed to work with ApplicationGroup storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class GroupManager : BaseManager, IGroupManager
    {
        public GroupManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Get one group from db by Id
        /// </summary>
        /// <param name="id">Id of company which you need</param>
        /// <returns>ViewModel of group from db</returns>
        public GroupViewModel Get(int id)
        {
            ApplicationGroup group = unitOfWork.ApplicationGroups.GetById(id);
            return mapper.Map<ApplicationGroup, GroupViewModel>(group);
        }

        /// <summary>
        /// Method for getting all groups from db
        /// </summary>
        /// <returns>IEnumerable of mapped to ViewModel objects</returns>
        public IEnumerable<GroupViewModel> GetGroups()
        {
            IEnumerable<ApplicationGroup> groups = unitOfWork.ApplicationGroups.GetAll();
            return mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<GroupViewModel>>(groups);
        }

        /// <summary>
        /// Method for inserting new group to db
        /// </summary>
        /// <param name="item">ViewModel of group</param>
        public bool Insert(GroupViewModel item)
        {
                ApplicationGroup group = mapper.Map<GroupViewModel, ApplicationGroup>(item);
                try
                {
                    unitOfWork.ApplicationGroups.Insert(group);
                    unitOfWork.Save();
                }
                catch(Exception)
                {
                    return false;
                }
                return true;
        }

        /// <summary>
        /// Update group in db
        /// </summary>
        /// <param name="item">ViewModel which need to update in db</param>
        public bool Update(GroupViewModel item)
        {
                var exisctGroup = unitOfWork.ApplicationGroups.GetById(item.Id);
                if (exisctGroup == null)
                {
                    return false;
                }
                ApplicationGroup group = mapper.Map<GroupViewModel, ApplicationGroup>(item);
                try
                {
                    unitOfWork.ApplicationGroups.Update(group);
                    unitOfWork.Save();
                }
                catch
                {
                    return false;
                }
                return true;
        }

        /// <summary>
        /// Delete group by Id
        /// </summary>
        /// <param name="id">Id of group which need to delete</param>
        public bool Delete(int id)
        {
            ApplicationGroup group = unitOfWork.ApplicationGroups.GetById(id);
            if (group == null)
            {
                return false;
            }

            unitOfWork.ApplicationGroups.Delete(group);
            unitOfWork.Save();
            return true;
        }
    }
}
