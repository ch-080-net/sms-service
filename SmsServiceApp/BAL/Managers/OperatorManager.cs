using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using AutoMapper;
using System.Linq;
using System.IO;

namespace BAL.Managers
{
    public class OperatorManager : BaseManager, IOperatorManager
    {
        public OperatorManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public IEnumerable<OperatorViewModel> GetAll()
        {
            var operators = unitOfWork.Operators.GetAll();

            var result = mapper.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>(operators);
            return result;
        }

        public bool Add(OperatorViewModel newOperator)
        {
            if (newOperator.Name == null || newOperator.Name == "")
                return false;
            var check = unitOfWork.Operators.Get(o => o.Name == newOperator.Name).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Operator>(newOperator);
            try
            {
                unitOfWork.Operators.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public OperatorViewModel GetById(int id)
        {
            var oper = unitOfWork.Operators.GetById(id);
            return mapper.Map<OperatorViewModel>(oper);
        }

        public bool Remove(int id)
        {
            var oper = unitOfWork.Operators.GetById(id);
            if (oper == null)
                return false;
            try
            {
                unitOfWork.Operators.Delete(oper);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Update(OperatorViewModel updatedOperator)
        {
            if (updatedOperator.Name == null || updatedOperator.Name == "")
                return false;
            var check = unitOfWork.Operators.Get(o => o.Name == updatedOperator.Name).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Operator>(updatedOperator);
            try
            {
                unitOfWork.Operators.Update(result);
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