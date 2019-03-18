using Model.ViewModels.AnswersCodeViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    public interface IAnswersCodeManager
    {
        IEnumerable<AnswersCodeViewModel> GetAnswersCodes(int companyId);
        AnswersCodeViewModel GetAnswersCodeById(int id);
        void Insert(AnswersCodeViewModel item, int companyId);
        void Update(AnswersCodeViewModel item);
        void Delete(int id);
    }
}
