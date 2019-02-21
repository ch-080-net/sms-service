using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;

namespace BAL.Managers
{
    public interface IOperatorManager
    {


        IEnumerable<OperatorViewModel> GetAllOperators();
    }
}
