using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using AutoMapper;
using Model.ViewModels.RecipientViewModels;

namespace BAL.Managers
{
    public class RecipientManager : BaseManager, IRecipientManager
    {
        public RecipientManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public void Delete(RecipientViewModel item)
        {
            Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
            unitOfWork.Recipients.Delete(recipient);
            unitOfWork.Save();
        }

        public IEnumerable<RecipientViewModel> GetRecipients()
        {
            IEnumerable<Recipient> recipients = unitOfWork.Recipients.GetAll();
            return mapper.Map<IEnumerable<Recipient>, IEnumerable<RecipientViewModel>>(recipients);
        }

        public void Insert(RecipientViewModel item)
        {
            Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
            unitOfWork.Recipients.Insert(recipient);
            unitOfWork.Save();
        }

        public void SetStateModified(RecipientViewModel item)
        {
            Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
            unitOfWork.Recipients.SetStateModified(recipient);
            unitOfWork.Save();
        }

        public void Update(RecipientViewModel item)
        {
            Recipient recipient = mapper.Map<RecipientViewModel, Recipient>(item);
            unitOfWork.Recipients.Update(recipient);
            unitOfWork.Save();
        }
    }
}
