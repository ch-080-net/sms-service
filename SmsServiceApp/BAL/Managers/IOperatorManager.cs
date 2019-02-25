using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;

namespace BAL.Managers
{
    public interface IOperatorManager
    {
        IEnumerable<OperatorViewModel> GetAll();
        OperatorViewModel GetById(int Id);
        bool Add(OperatorViewModel NewOperator);
        bool Update(OperatorViewModel UpdatedOperator);
        bool Remove(int Id);
        IEnumerable<OperatorViewModel> FindByName(string Name);

    }
}
