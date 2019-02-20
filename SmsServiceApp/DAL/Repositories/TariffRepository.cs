using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
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

		public void ChangeTariffLimit(Tariff currentTariff, int newLimit, string userRole)
		{
			if (userRole == "Admin")
			{
				Tariff t = context.Tariffs.Find(currentTariff.Id);
				t.Limit = newLimit;
				context.SaveChanges();
			}
			else
			{
				throw new UnauthorizedAccessException();
			}
		}
	}
}
