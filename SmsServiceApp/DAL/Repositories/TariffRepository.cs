using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
	public class TariffRepository : BaseRepository<Tariff> , ITariffRepository
	{

        public TariffRepository(ApplicationDbContext context) :  base(context)
		{
		}

        public IEnumerable<Tariff> GetByOperatorId(int operatorId)
        {
            return base.Get(filter: item => item.OperatorId == operatorId);
        }

		public void ChangeTariffPricing(Tariff currentPrice, decimal newPrice, string userRole)
		{
            Tariff p = context.Tariffs.Find(currentPrice.Price);
            p.Price = newPrice;
            context.SaveChanges();
		}

	}
}
