using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;

namespace Model.Interfaces
{
    public interface IOperatorManager
    {
        OperatorViewModel GetById(int id);
        bool Add(OperatorViewModel newOperator);
        bool Update(OperatorViewModel updatedOperator);
        bool Remove(int id);
        IEnumerable<OperatorViewModel> GetAll();
        bool AddLogo(LogoViewModel logo);
        Page GetPage(PageState pageState);
    }
}

