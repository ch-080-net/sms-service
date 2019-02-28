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
        public bool Add(CodeViewModel newCode)
        {
            if (newCode.OperatorCode == null || newCode.OperatorCode == "")
                return false;
            var check = unitOfWork.Codes.Get(o => o.OperatorCode == newCode.OperatorCode).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Code>(newCode);
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

        public CodeViewModel GetById(int id)
        {
            var oper = unitOfWork.Codes.GetById(id);
            return mapper.Map<CodeViewModel>(oper);
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

        public bool Update(CodeViewModel updatedCode)
        {
            if (updatedCode.OperatorCode == null || updatedCode.OperatorCode == "")
                return false;

            var check = unitOfWork.Codes.Get(o => o.OperatorCode == updatedCode.OperatorCode).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Code>(updatedCode);
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

        public Page GetPage(PageState pageState)
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
