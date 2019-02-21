using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace DAL.Repositories
{
	public class TariffRepository : BaseRepository<Tariff> , ITariffRepository
	{
		internal DbSet<Tariff> dbSet;

		public TariffRepository(ApplicationDbContext context) :  base(context)
		{
		}

        public IEnumerable<Tariff> GetByOperatorId(int operatorId)
        {
            return base.Get(filter: item => item.OperatorId == operatorId);
        }

		[Authorize(Roles = "Admin")]
		public void ChangeTariffLimit(Tariff currentTariff, int newLimit)
		{
			Tariff t = context.Tariffs.Find(currentTariff.Id);
			t.Limit = newLimit;
			context.SaveChanges();
		}

		public void ChangeTariffPricing(Tariff currentTariff, decimal newPrice, string userRole)
		{
			throw new NotImplementedException();
		}

	}
}
