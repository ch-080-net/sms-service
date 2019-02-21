using Microsoft.AspNetCore.Authorization;
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

		[Authorize(Roles = "Admin")]
		public void ChangeTariffLimit(Tariff currentTariff, int newLimit)
		{
			Tariff t = context.Tariffs.Find(currentTariff.Id);
			t.Limit = newLimit;
			context.SaveChanges();
		}
	}
}
