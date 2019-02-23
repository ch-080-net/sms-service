using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using AutoMapper;

namespace BAL.Managers
{
    public class OperatorManager : BaseManager, IOperatorManager
    {
        public OperatorManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public bool Add(OperatorViewModel NewOperator)
        {
            var result = mapper.Map<Operator>(NewOperator);
            try
            {
                unitOfWork.Operators.Insert(result);
            }
            catch
            {
                return false;
            }
            unitOfWork.Save();
            return true;
        }

        public IEnumerable<OperatorViewModel> FindByName(string Name)
        {
            var operators = unitOfWork.Operators.Get(o => o.Name == Name);
            var result = new List<OperatorViewModel>();
            foreach (var o in operators)
            {
                result.Add(mapper.Map<OperatorViewModel>(o));
            }
            return result;
        }

        public IEnumerable<OperatorViewModel> GetAll()
        {
            var operators = unitOfWork.Operators.GetAll();
            var result = new List<OperatorViewModel>();
            foreach (var o in operators)
            {
                result.Add(mapper.Map<OperatorViewModel>(o));
            }
            return result;
        }

        public OperatorViewModel GetById(int Id)
        {
            var oper = unitOfWork.Operators.GetById(Id);
            return mapper.Map<OperatorViewModel>(oper);
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
            var result = mapper.Map<Operator>(UpdatedOperator);
            try
            {
                unitOfWork.Operators.Update(result);
            }
            catch
            {
                return false;
            }
            unitOfWork.Save();
            return true;
        }
    }
}
