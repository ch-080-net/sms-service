using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace Model.Interfaces
{
   public interface ITariffRepository : IBaseRepository<Tariff>
    {
        IEnumerable<Tariff> GetByOperatorId(int operatorId);
    }
}
