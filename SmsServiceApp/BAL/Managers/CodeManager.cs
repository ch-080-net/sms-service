using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using Model.Interfaces;
using Model.ViewModels.CodeViewModels;
using AutoMapper;
using System.Linq;
using Model.DTOs;

namespace BAL.Managers
{
    /// <summary>
    /// Manger for CRUD operations on Codes
    /// </summary>
    public class CodeManager : BaseManager, ICodeManager
    {
        public CodeManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        /// <summary>
        /// Transform CodeViewModel <paramref name="newCode"/> to Code and insert it to Codes table of DB
        /// </summary>
        /// <param name="newCode">Should contain not null or empty OperatorCode and OperatorId</param>
        /// <returns>true, if transaction succesfull; false if not</returns>
        public TransactionResultDTO Add(CodeViewModel newCode)
        {
            if (newCode.OperatorCode == null || newCode.OperatorCode == "")
                return new TransactionResultDTO() {Success = false, Details = "Code cannot be empty" };
            var check = unitOfWork.Codes.Get(o => o.OperatorCode == newCode.OperatorCode).FirstOrDefault();
            if (check != null)
                return new TransactionResultDTO() { Success = false, Details = "Code belongs to another operator" };
            var result = mapper.Map<Code>(newCode);
            try
            {
                unitOfWork.Codes.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Get CodeViewModel by Id
        /// </summary>
        /// <param name="id">Id of Code in Codes table</param>
        /// <returns>Provides empty CodeViewModel if provided null</returns>
        public CodeViewModel GetById(int id)
        {
            var oper = unitOfWork.Codes.GetById(id);
            return mapper.Map<CodeViewModel>(oper);
        }


        /// <summary>
        /// Remonve entry in Codes table with <paramref name="id"/>
        /// </summary>
        /// <returns>true, if transaction succesfull; false if not</returns>
        public TransactionResultDTO Remove(int id)
        {
            var code = unitOfWork.Codes.GetById(id);
            if (code == null)
                return new TransactionResultDTO() { Success = false, Details = "Code already removed" };
            try
            {
                unitOfWork.Codes.Delete(code);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Transform CodeViewModel <paramref name="updatedCode"/> to Code and update corresponding row in Codes table of DB
        /// </summary>
        /// <param name="updatedCode">
        /// Should contain not null or empty OperatorCode; Id and OperatorId of existing entries in Codes and Operators tables
        /// OperatorCode must be unique
        /// </param>
        /// <returns>true, if transaction succesfull; false if not</returns>
        public TransactionResultDTO Update(CodeViewModel updatedCode)
        {
            if (updatedCode.OperatorCode == null || updatedCode.OperatorCode == "")
                return new TransactionResultDTO() { Success = false, Details = "Code cannot be empty" };

            var check = unitOfWork.Codes.Get(o => o.OperatorCode == updatedCode.OperatorCode).FirstOrDefault();
            if (check != null)
                return new TransactionResultDTO() { Success = false, Details = "Code belongs to another operator" };
            var result = mapper.Map<Code>(updatedCode);
            try
            {
                unitOfWork.Codes.Update(result);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Get Page, which corresponds to <paramref name="pageState"/> 
        /// </summary>
        /// <param name="pageState">Represents page state, including search querry, page, number of pages, entries on one page</param>
        /// <returns>Object, which contains valid page state and corresponding enumeration of CodesViewModel</returns>
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
