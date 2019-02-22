using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;
using AutoMapper;

namespace BAL.Managers
{
    public class RecipientManager : BaseManager, IRecipientManager
    {
        public RecipientManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public IEnumerable<Recipient> GetRecipients()
        {
            return unitOfWork.Recipients.GetAll();
        }
    }
}
