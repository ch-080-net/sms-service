using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using Model.ViewModels.AdminStatisticViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
   public class AdminStatisticManager : BaseManager, IAdminStatisticManager
    {
        public AdminStatisticManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public IEnumerable<AdminStatisticViewModel> GetAll()
        {
            var groups = unitOfWork.ApplicationGroups.GetAll();

            var result = mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<AdminStatisticViewModel>>(groups);
            return result;
        }
       

      

    }
}
