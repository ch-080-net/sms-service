using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;
using Model.DTOs;

namespace BAL.Managers
{
    public interface IOperatorManager
    {
        OperatorViewModel GetById(int id);
		OperatorViewModel GetByName(string name);
		TransactionResultDTO Add(OperatorViewModel newOperator);
        TransactionResultDTO Update(OperatorViewModel updatedOperator);
        TransactionResultDTO Remove(int id);
        IEnumerable<OperatorViewModel> GetAll();
        TransactionResultDTO AddLogo(LogoViewModel logo);
        Page GetPage(PageState pageState);
    }
}

