using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.CodeViewModels;

namespace Model.Interfaces
{
    public interface ICodeManager
    {
        CodeViewModel GetById(int id);
        bool Add(CodeViewModel newCode);
        bool Update(CodeViewModel updatedCode);
        bool Remove(int id);      
        Page GetPage(PageState pageState);
    }
}
