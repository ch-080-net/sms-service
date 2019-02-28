using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.CodeViewModels;

namespace Model.Interfaces
{
    public interface ICodeManager
    {
        CodeViewModel GetById(int Id);
        bool Add(CodeViewModel NewCode);
        bool Update(CodeViewModel UpdatedCode);
        bool Remove(int Id);
        IEnumerable<CodeViewModel> GetPage(int OperatorId, int Page = 1, int NumOfElements = 20, string SearchQuerry = "");
        int GetNumberOfPages(int OperatorId, int NumOfElements = 20, string SearchQueery = "");
        Page GetCurrentPage(PageState pageState);
    }
}
