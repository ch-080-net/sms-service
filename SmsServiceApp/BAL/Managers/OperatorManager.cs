using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;

namespace BAL.Managers
{
    public class OperatorManager : BaseManager, IOperatorManager
    {
        public OperatorManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<OperatorViewModel> GetAllOperators()
        {
            var operators = unitOfWork.Operators.GetAll();
            throw new NotImplementedException();
        }

    }
}
