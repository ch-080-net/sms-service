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

        /// <summary>
        /// Delete AnswersCode in db
        /// </summary>
        /// <param name="id">id of AnswersCode for deleting</param>
        public void Delete(int id)
        {
            try
            {
                AnswersCode answersCode = unitOfWork.AnswersCodes.GetById(id);
                unitOfWork.AnswersCodes.Delete(answersCode);
                unitOfWork.Save();
            }
            catch(Exception ex)
            {
                throw new Exception("Exception from delete method", ex); 
            }
        }

        /// <summary>
        /// Get AnswersCode by id
        /// </summary>
        /// <param name="id">id of AnswersCode</param>
        /// <returns>AnswersCodeViewModel</returns>
        public AnswersCodeViewModel GetAnswersCodeById(int id)
        {
            AnswersCode answersCode = unitOfWork.AnswersCodes.GetById(id);
            return mapper.Map<AnswersCodeViewModel>(answersCode);
        }

        /// <summary>
        /// Get AnswersCodes by company id
        /// </summary>
        /// <param name="companyId">id of company</param>
        /// <returns>List of AnswersCodeViewModel</returns>
        public IEnumerable<AnswersCodeViewModel> GetAnswersCodes(int companyId)
        {
            IEnumerable<AnswersCode> answersCodes = unitOfWork.AnswersCodes.Get(r => r.CompanyId == companyId);
            return mapper.Map<IEnumerable<AnswersCode>, List<AnswersCodeViewModel>>(answersCodes);
        }

        /// <summary>
        /// Insert new AnswersCode into db
        /// </summary>
        /// <param name="item">AnswersCode for inserting</param>
        /// <param name="companyId"></param>
        public void Insert(AnswersCodeViewModel item, int companyId)
        {
            try
            {
                AnswersCode answersCode = mapper.Map<AnswersCodeViewModel, AnswersCode>(item);
                answersCode.CompanyId = companyId;
                unitOfWork.AnswersCodes.Insert(answersCode);
                unitOfWork.Save();
            }
            catch(Exception ex)
            {
				throw new Exception("Exception from insert method", ex);
			}
        }

        /// <summary>
        /// Update AnswersCode
        /// </summary>
        /// <param name="item">AnswersCode for updating</param>
        public void Update(AnswersCodeViewModel item)
        {
            try
            {
                AnswersCode answersCode = mapper.Map<AnswersCodeViewModel, AnswersCode>(item);
                unitOfWork.AnswersCodes.Update(answersCode);
                unitOfWork.Save();
            }
            catch(Exception ex)
            {
				throw new Exception("Exception from update method", ex);
			}
        }
    }
}
