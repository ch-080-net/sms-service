using Model.Interfaces;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace DAL.Repositories
{
	public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(ApplicationDbContext context) : base(context)
		{
		}

		public bool IsLimitExceeded(int messageCount, Company currentCompany)
		{
			Tariff companyTariff = context.Tariffs.Find(currentCompany.TariffId);

			if (messageCount > companyTariff.Limit)
				return true;

			return false;
		}

		public int GetTariffLimit(Company company)
		{
			return context.Tariffs.Find(company.TariffId).Limit;
		}
	}
}
