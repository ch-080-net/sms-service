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
	}
}
