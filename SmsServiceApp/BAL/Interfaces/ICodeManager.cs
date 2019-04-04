using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.CodeViewModels;
using Model.DTOs;

namespace BAL.Managers
{
    public interface ICodeManager
    {
        CodeViewModel GetById(int id);
        TransactionResultDTO Add(CodeViewModel newCode);
        TransactionResultDTO Update(CodeViewModel updatedCode);
        TransactionResultDTO Remove(int id);      
        Page GetPage(PageState pageState);
    }
}
