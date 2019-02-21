using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using AutoMapper;

namespace BAL.Managers
{
    public class OperatorManager : BaseManager, IOperatorManager
    {
        public OperatorManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public IEnumerable<OperatorViewModel> GetAllOperators()
        {
            var operators = unitOfWork.Operators.GetAll();

            foreach (var o in operators)
            {

            }
            throw new NotImplementedException();
        }

    }
}
