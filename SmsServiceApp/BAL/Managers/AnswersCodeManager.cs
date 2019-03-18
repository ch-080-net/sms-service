using AutoMapper;
using BAL.Interfaces;
using Model.Interfaces;
using Model.ViewModels.AnswersCodeViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    public class AnswersCodeManager : BaseManager, IAnswersCodeManager
    {
        public AnswersCodeManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public void Delete(int id)
        {
            AnswersCode recipient = unitOfWork.AnswersCodes.GetById(id);
            unitOfWork.AnswersCodes.Delete(recipient);
            unitOfWork.Save();
        }

        public AnswersCodeViewModel GetAnswersCodeById(int id)
        {
            AnswersCode answersCode = unitOfWork.AnswersCodes.GetById(id);
            return mapper.Map<AnswersCodeViewModel>(answersCode);
        }

        public IEnumerable<AnswersCodeViewModel> GetAnswersCodes(int companyId)
        {
            IEnumerable<AnswersCode> answersCodes = unitOfWork.AnswersCodes.Get(r => r.CompanyId == companyId);
            return mapper.Map<IEnumerable<AnswersCode>, List<AnswersCodeViewModel>>(answersCodes);
        }

        public void Insert(AnswersCodeViewModel item, int companyId)
        {
            AnswersCode answersCode = mapper.Map<AnswersCodeViewModel, AnswersCode>(item);
            answersCode.CompanyId = companyId;
            unitOfWork.AnswersCodes.Insert(answersCode);
            unitOfWork.Save();
        }

        public void Update(AnswersCodeViewModel item)
        {
            AnswersCode answersCode = mapper.Map<AnswersCodeViewModel, AnswersCode>(item);
            unitOfWork.AnswersCodes.Update(answersCode);
            unitOfWork.Save();
        }
    }
}
