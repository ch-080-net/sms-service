using Model.Interfaces;
using System.Collections.Generic;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository for Tariffs
    /// Inherited from BaseManager and have additional method for getting Tariff by OperatorId
    /// </summary>
    public class TariffRepository : BaseRepository<Tariff> , ITariffRepository
	{

        public TariffRepository(ApplicationDbContext context) :  base(context)
		{
		}
        /// <summary>
        /// Get Tariffs by OperatorID
        /// </summary>
        /// <param name="operatorId">OperatorsId</param>
        /// <returns>Tariff for Operator</returns>
        public IEnumerable<Tariff> GetByOperatorId(int operatorId)
        {
            return base.Get(filter: item => item.OperatorId == operatorId);
        }
        /// <summary>
        /// Change price for Tariff
        /// </summary>
        /// <param name="currentTariff">Current Tariff</param>
        /// <param name="newPrice">New Price for Tariff</param>
        /// <param name="userRole">Get user Role</param>
		public void ChangeTariffPricing(Tariff currentTariff, decimal newPrice, string userRole)
		{
            Tariff p = context.Tariffs.Find(currentTariff.Price);
            p.Price = newPrice;
            context.SaveChanges();
		}

	}
}
