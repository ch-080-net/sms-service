using Model.Interfaces;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
	public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
