using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace Model.Interfaces
{
	public interface ICompanyRepository : IBaseRepository<Company>
	{
		bool IsLimitExceeded(int messageCount, Company currentCompany);
		int GetTariffLimit(Company company);
	}
}
