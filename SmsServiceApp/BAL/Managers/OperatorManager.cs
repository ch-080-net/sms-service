using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using AutoMapper;
using System.Linq;

namespace BAL.Managers
{
    public class OperatorManager : BaseManager, IOperatorManager
    {
        public OperatorManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public bool Add(OperatorViewModel NewOperator)
        {
            var check = unitOfWork.Operators.Get(o => o.Name == NewOperator.Name).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Operator>(NewOperator);
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

        public OperatorViewModel GetById(int Id)
        {
            var oper = unitOfWork.Operators.GetById(Id);
            return mapper.Map<OperatorViewModel>(oper);
        }

        public int GetNumberOfPages(int NumOfElements = 20, string SearchQuerry = "")
        {
            return (unitOfWork.Operators.Get(o => o.Name.Contains(SearchQuerry)).Count() / (NumOfElements + 1)) + 1;
        }

        public IEnumerable<OperatorViewModel> GetPage(int Page = 1, int NumOfElements = 20, string SearchQuerry = "")
        {
            var operators = unitOfWork.Operators.Get(o => o.Name.Contains(SearchQuerry),
                o => o.OrderByDescending(s => s.Id));
            operators = operators.Skip(NumOfElements * (Page - 1)).Take(NumOfElements);
            var result = mapper.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>(operators);
            return result;
        }

        public bool Remove(int Id)
        {
            var oper = unitOfWork.Operators.GetById(Id);
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

        public bool Update(OperatorViewModel UpdatedOperator)
        {
            var check = unitOfWork.Operators.Get(o => o.Name == UpdatedOperator.Name).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Operator>(UpdatedOperator);
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
    }
}