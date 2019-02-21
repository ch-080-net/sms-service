using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public interface ITariffManager
    {
        IEnumerable<Tariff> GetByOperatorId();
        IEnumerable<Tariff> GetTariffs();
    }
}
