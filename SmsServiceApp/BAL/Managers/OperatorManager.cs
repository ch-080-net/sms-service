using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using AutoMapper;
using System.Linq;
using System.IO;
using Model.DTOs;

namespace BAL.Managers
{
    /// <summary>
    /// Manger for CRUD operations on Operators
    /// </summary>
    public class OperatorManager : BaseManager, IOperatorManager
    {
        public OperatorManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        /// <summary>
        /// Get all operators from DB and transform them to OperatorViewModel
        /// </summary>
        public IEnumerable<OperatorViewModel> GetAll()
        {
            var operators = unitOfWork.Operators.GetAll();

            var result = mapper.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>(operators);
            return result;
        }

        /// <summary>
        /// Transform OperatorViewModel <paramref name="newOperator"/> to Code and insert it to Codes table of DB
        /// </summary>
        /// <param name="newOperator">
        /// Should contain not null or empty Name.
        /// Name must be unique
        /// </param>
        /// <returns>true, if transaction succesfull; false if not</returns>
        public TransactionResultDTO Add(OperatorViewModel newOperator)
        {
            if (newOperator.Name == null || newOperator.Name == "")
                return new TransactionResultDTO() { Success = false, Details = "Operator name cannot be empty"};
            var check = unitOfWork.Operators.Get(o => o.Name == newOperator.Name).FirstOrDefault();
            if (check != null)
                return new TransactionResultDTO() { Success = false, Details = "Operator with this name already exist" };
            var result = mapper.Map<Operator>(newOperator);
            try
            {
                unitOfWork.Operators.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Get OperatorViewModel by Id
        /// </summary>
        /// <param name="id">Id of Operator in Operators table</param>
        /// <returns>Provides empty OperatorViewModel if provided null</returns>
        public OperatorViewModel GetById(int id)
        {
            var oper = unitOfWork.Operators.GetById(id);
            return mapper.Map<OperatorViewModel>(oper);
        }

        /// <summary>
        /// Remove entry from Operators table with corresponding <paramref name="id"/>
        /// </summary>
        /// <returns>true, if transaction succesfull; false if not</returns>
        public TransactionResultDTO Remove(int id)
        {
            var oper = unitOfWork.Operators.GetById(id);
            if (oper == null)
                return new TransactionResultDTO() { Success = false, Details = "Operator already removed" };
            try
            {
                unitOfWork.Operators.Delete(oper);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Transform OperatorViewModel <paramref name="updatedOperator"/> to Operator and update corresponding row in Operators table of DB
        /// </summary>
        /// <param name="updatedOperator">
        /// Should contain not null or empty Name and Id of existing Operators table entry.
        /// Name must be unique
        /// </param>
        /// <returns>true, if transaction succesfull; false if not</returns>
        public TransactionResultDTO Update(OperatorViewModel updatedOperator)
        {
            if (updatedOperator.Name == null || updatedOperator.Name == "")
                return new TransactionResultDTO() { Success = false, Details = "Operator name cannot be empty" };
            var check = unitOfWork.Operators.Get(o => o.Name == updatedOperator.Name).FirstOrDefault();
            if (check != null)
                return new TransactionResultDTO() { Success = false, Details = "Operator name already exist" };
            var result = mapper.Map<Operator>(updatedOperator);
            try
            {
                unitOfWork.Operators.Update(result);
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
        /// <returns>Object, which contains valid page state and corresponding enumeration of OperatorViewModel</returns>
        public Page GetPage(PageState pageState)
        {
            if (pageState == null)
                return null;

            if (pageState.SearchQuerry == null)
                pageState.SearchQuerry = "";

            var operators = unitOfWork.Operators.Get(o => o.Name.Contains(pageState.SearchQuerry));

            if (pageState.OperatorsOnPage < 1)
                pageState.OperatorsOnPage = 10;

            pageState.LastPage = operators.Count() / pageState.OperatorsOnPage
                + (((operators.Count() % pageState.OperatorsOnPage) == 0) ? 0 : 1);

            if (pageState.Page > pageState.LastPage)
                pageState.Page = pageState.LastPage;

            if (pageState.Page < 1)
                pageState.Page = 1;

            var result = new Page();

            result.OperatorList = mapper.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>
                (operators.Skip((pageState.Page - 1) * pageState.OperatorsOnPage).Take(pageState.OperatorsOnPage));

            result.PageState = pageState;

            return result;
        }

        public bool AddLogo(LogoViewModel logo)
        {
            if (logo.Logo == null)
                return false;

            var oper = unitOfWork.Operators.GetById(logo.OperatorId);
            if (oper == null)
                return false;

            byte[] imgData = null;
            using (var binReader = new BinaryReader(logo.Logo.OpenReadStream()))
            {
                imgData = binReader.ReadBytes((int)logo.Logo.Length);
            }
            oper.Logo = imgData;
            try
            {
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}