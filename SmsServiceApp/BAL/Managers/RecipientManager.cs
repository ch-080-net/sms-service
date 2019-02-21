using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using Model.Interfaces;

namespace BAL.Managers
{
    public class RecipientManager : BaseManager, IRecipientManager
    {
        public RecipientManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<Recipient> GetRecipients()
        {
            return unitOfWork.Recipients.GetAll();
        }
    }
}
