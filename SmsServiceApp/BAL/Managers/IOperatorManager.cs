using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;

namespace BAL.Managers
{
    public interface IOperatorManager
    {
        OperatorViewModel GetById(int Id);
        bool Add(OperatorViewModel NewOperator);
        bool Update(OperatorViewModel UpdatedOperator);
        bool Remove(int Id);
        IEnumerable<OperatorViewModel> GetPage(int Page = 1, int NumOfElements = 20, string SearchQuerry = "");
        int GetNumberOfPages(int NumOfElements = 20, string SearchQuerry = "");
        bool AddLogo(LogoViewModel Logo);
    }
}

