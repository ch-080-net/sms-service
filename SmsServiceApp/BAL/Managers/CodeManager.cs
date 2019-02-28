using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.CodeViewModels;
using AutoMapper;
using System.Linq;

namespace BAL.Managers
{
    public class CodeManager : BaseManager, ICodeManager
    {
        public CodeManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        public bool Add(CodeViewModel NewCode)
        {
            if (NewCode.OperatorCode == null || NewCode.OperatorCode == "")
                return false;
            var check = unitOfWork.Codes.Get(o => o.OperatorCode == NewCode.OperatorCode).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Code>(NewCode);
            try
            {
                unitOfWork.Codes.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public CodeViewModel GetById(int Id)
        {
            var oper = unitOfWork.Codes.GetById(Id);
            return mapper.Map<CodeViewModel>(oper);
        }

        public int GetNumberOfPages(int OperatorId, int NumOfElements = 20, string SearchQuerry = "")
        {
            if (NumOfElements < 1)
                NumOfElements = 20;
            if (SearchQuerry == null)
                SearchQuerry = "";

            return (unitOfWork.Codes.Get(o => o.OperatorId == OperatorId && o.OperatorCode.Contains(SearchQuerry))
                .Count() / (NumOfElements + 1)) + 1;
        }

        public IEnumerable<CodeViewModel> GetPage(int OperatorId, int Page = 1, int NumOfElements = 20, string SearchQuerry = "")
        {
            if (Page < 1)
                Page = 1;
            if (NumOfElements < 1)
                NumOfElements = 20;
            if (SearchQuerry == null)
                SearchQuerry = "";

            var codes = unitOfWork.Codes.Get(o => o.OperatorId == OperatorId && o.OperatorCode.Contains(SearchQuerry),
                o => o.OrderBy(s => s.OperatorCode));
            codes = codes.Skip(NumOfElements * (Page - 1)).Take(NumOfElements);
            var result = mapper.Map<IEnumerable<Code>, IEnumerable<CodeViewModel>>(codes);
            return result;
        }

        public bool Remove(int Id)
        {
            var code = unitOfWork.Codes.GetById(Id);
            if (code == null)
                return false;
            try
            {
                unitOfWork.Codes.Delete(code);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Update(CodeViewModel UpdatedCode)
        {
            if (UpdatedCode.OperatorCode == null || UpdatedCode.OperatorCode == "")
                return false;

            var check = unitOfWork.Codes.Get(o => o.OperatorCode == UpdatedCode.OperatorCode).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Code>(UpdatedCode);
            try
            {
                unitOfWork.Codes.Update(result);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Page GetCurrentPage(PageState pageState)
        {
            if (pageState == null)
                return null;

            if (pageState.SearchQuerry == null)
                pageState.SearchQuerry = "";

            if (pageState.OperatorName == null)
                pageState.OperatorName = unitOfWork.Operators.GetById(pageState.OperatorId).Name;

            var codes = unitOfWork.Codes.Get(c => c.OperatorId == pageState.OperatorId
                && c.OperatorCode.Contains(pageState.SearchQuerry));

            if (pageState.CodesOnPage < 1)
                pageState.CodesOnPage = 10;

            pageState.LastPage = codes.Count() / pageState.CodesOnPage
                + (((codes.Count() % pageState.CodesOnPage) == 0) ? 0 : 1);

            if (pageState.Page > pageState.LastPage)
                pageState.Page = pageState.LastPage;

            if (pageState.Page < 1)
                pageState.Page = 1;

            var result = new Page();

            result.CodeList = mapper.Map<IEnumerable<Code>, IEnumerable<CodeViewModel>>
                (codes.Skip((pageState.Page - 1) * pageState.CodesOnPage).Take(pageState.CodesOnPage));

            result.PageState = pageState;

            return result;
        }
    }
}
