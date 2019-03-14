using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using AutoMapper;
using Model.DTOs;
using System.Threading.Tasks;
using System.Linq;
using WebCustomerApp.Models;
using Model.ViewModels.ChartsViewModels;

namespace BAL.Managers
{
    public class ChartsManager : BaseManager, IChartsManager
    {
        public ChartsManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public PieChart GetPieChart(int campaignId, string userId)
        {
            var campaign = unitOfWork.Charts.Get(pcc => pcc.Id == campaignId
                && pcc.ApplicationGroup.ApplicationUsers.Any(au => au.Id == userId)).FirstOrDefault();

            return mapper.Map<Company, PieChart>(campaign);
        }

        public StackedChart GetStackedChart(int campaignId, string userId)
        {
            var campaign = unitOfWork.Charts.Get(pcc => pcc.Id == campaignId
                && pcc.ApplicationGroup.ApplicationUsers.Any(au => au.Id == userId)).FirstOrDefault();

            return mapper.Map<Company, StackedChart>(campaign);
        }
    }
}
