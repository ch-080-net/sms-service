using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace DAL.Repositories
{
  public class TariffRepository :BaseRepository<Tariff>,ITariffRepository 
    {
        public TariffRepository(ApplicationDbContext sendingDbContext) : base(sendingDbContext) { }

        public IEnumerable<Tariff> GetByOperatorId(int operatorId)
        {
            return base.Get(filter: item => item.OperatorId == operatorId);
        }
    }
}
